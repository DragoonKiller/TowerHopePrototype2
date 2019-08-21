using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tower.Skills
{
    using Utils;
    using Systems;
    using Components;
    using Tower.Global;

    [Serializable]
    public class SampleAttack : ISkillConfig
    {
        [Serializable]
        public class Phase { [SerializeField] public Sprite[] sprites; }
        public Phase[] phases;

        public GameObject animSource;

        [Tooltip("每个攻击阶段的时间间隔.")]
        public float phaseDelay;

        [Tooltip("每个动画帧占用的时间.")]
        public float frameTime;

        [Tooltip("动画缩放.")]
        public float scale;

        [Tooltip("每帧速度会被乘以这个常数.")]
        public float decelerateRate;

        // [Tooltip("每次发动攻击会向固定方向加速移动.")]
        // public float attackMoveSpeed;


        public bool TryGetState(Role role, out StateMachine stateMachine)
        {
            var anim = GameObject.Instantiate(animSource, role.transform);
            anim.transform.localPosition = Vector3.zero;
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localScale = Vector2.one * scale;

            // 根据光标位置翻转图像.
            if(ExCursor.worldPos.x < role.transform.position.x)
                anim.transform.localScale = anim.transform.localScale.X(anim.transform.localScale.x * -1.0f);

            stateMachine = new STM() {
                data = this,
                role = role,
                anim = anim.GetComponent<SpriteRenderer>(),
                dir = ExCursor.worldPos.x < role.transform.position.x ? -1f : 1f
            };
            return true;
        }

        private class STM : StateMachine
        {
            public SampleAttack data;
            public Role role;
            public SpriteRenderer anim;
            public float dir;

            /// <summary>
            /// 每迭代一次就步进一帧.
            /// 返回: 当前阶段是否结束.
            /// </summary>
            public IEnumerator<bool> StepAnim()
            {
                foreach(var phase in data.phases)
                {
                    foreach(var sprite in phase.sprites)
                    {
                        anim.sprite = sprite;
                        yield return false;
                    }

                    yield return true;
                }
            }

            public override IEnumerator<Transfer> Step()
            {
                float t = 0f;
                var animState = StepAnim();

                role.rd.velocity = dir * Vector2.right * role.action.groundHoriSpeed;
                
                while(true)
                {
                    // 移动速度减少.
                    role.rd.velocity = role.action.NextVelocity(role.rd.velocity, Vector2.zero, data.decelerateRate, Time.deltaTime);

                    t += Time.deltaTime;
                    while(t >= data.frameTime)
                    {
                        t -= data.frameTime;
                        // 没有下一帧了, 结束.
                        if(!animState.MoveNext()) yield break;

                        // 以该状态帧等待一段时间.
                        // 记录这段时间中的指令.
                        bool shouldContinue = false;
                        if(animState.Current)
                        {
                            while(t <= data.phaseDelay)
                            {
                                yield return Pass();
                                t += Time.deltaTime;
                                if(CommandQueue.Get(KeyBinding.inst.attack)) shouldContinue = true;
                            }

                            // 当前阶段结束, 而且没有进行下一阶段的指令, 结束.
                            if(!shouldContinue) yield break;
                            t -= data.phaseDelay;
                            role.rd.velocity = dir * Vector2.right * role.action.groundHoriSpeed;
                        }

                    }
                    yield return Pass();
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                GameObject.Destroy(anim.gameObject);
            }
        }
    }
}