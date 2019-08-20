using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tower.Skills
{
    using Systems;
    using Components;

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


        public bool TryGetState(Role role, out StateMachine stateMachine)
        {
            var anim = GameObject.Instantiate(animSource, role.transform);
            anim.transform.localPosition = Vector3.zero;
            anim.transform.localRotation = Quaternion.identity;

            stateMachine = new STM() {
                data = this,
                role = role,
                anim = anim.GetComponent<SpriteRenderer>(),
            };
            return true;
        }

        private class STM : StateMachine
        {
            public SampleAttack data;
            public Role role;
            public SpriteRenderer anim;

            /// <summary>
            /// 每迭代一次就步进一帧.
            /// </summary>
            public IEnumerator<object> StepAnim()
            {
                foreach(var phase in data.phases)
                {
                    foreach(var sprite in phase.sprites)
                    {
                        anim.sprite = sprite;
                        yield return null;
                    }
                }
            }

            public override IEnumerator<Transfer> Step()
            {
                float t = 0f;
                var animState = StepAnim();
                while(true)
                {
                    t += Time.deltaTime;
                    while(t >= data.frameTime)
                    {
                        t -= data.frameTime;
                        if(!animState.MoveNext()) yield break;
                    }
                    yield return Pass();
                }
            }
        }
    }
}