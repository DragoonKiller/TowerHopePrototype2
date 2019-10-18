using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Skills
{
    using Tower.Components;

    [Serializable]
    public class WindRush : ISkillConfig
    {
        [Tooltip("在这个时限之内, 技能不会因为撞墙而中断.")]
        public float preserveTime;

        [Tooltip("法力消耗.")]
        public float magicCost;
        
        [Tooltip("突进的速度-时间曲线.")]
        public AnimationCurve curve;

        [Tooltip("基础突进时间.")]
        public float duration;
        
        [Tooltip("基础突进速度.")]
        public float speed;
        
        [Tooltip("技能结束后, 保留多少倍速度.")]
        public float restSpeedMult;

        [Tooltip("技能因为撞击而结束后, 保留多少倍速度.")]
        public float restSpeedCollisionMult;


        public bool TryGetState(Role role, out StateMachine stateMachine)
        {
            stateMachine = null;

            if(role.magic == null) return false;
            if(role.magic.TryUseMagic(magicCost))
            {
                stateMachine = new STM() {
                    data = this,
                    role = role,
                };
                return true;
            }
            return false;
        }

        private class STM : StateMachine
        {
            public WindRush data;
            public Role role;
            const int contactInfoExtractionLimit = 20;
            readonly ContactPoint2D[] res = new ContactPoint2D[contactInfoExtractionLimit];
            public override IEnumerator<Transfer> Step()
            {
                var from = role.transform.position.ToVec2();
                var dir = ExCursor.worldPos - from;
                var beginTime = Time.time;
                Debug.DrawLine(from, from + dir);
                while(true)
                {
                    // 如果超时了, 把速度设置到基础速度的一个倍数, 然后退出.
                    if(Time.time - beginTime >= data.duration)
                    {
                        role.rd.velocity = dir.normalized * data.speed * data.restSpeedMult;
                        yield return Trans(role.action.GetFlyState());
                    }
                    
                    yield return Pass();
                    
                    var ct = role.rd.GetContacts(new ContactFilter2D() {
                        layerMask = LayerMask.GetMask("Terrain")
                    }, res);
                    
                    var rate = (Time.time - beginTime) / data.duration;
                    var nextV = data.curve.Evaluate(rate) * data.speed * dir.normalized;
                    role.rd.velocity = nextV;
                    
                    DebugDraw.Circle(role.transform.position, data.curve.Integral(rate, 1.0f) * data.speed * data.duration, Color.red);
                    
                    // 如果撞到了东西, 结束...
                    if(Time.time - beginTime > data.preserveTime && ct != 0)
                    {
                        role.rd.velocity = dir.normalized * data.speed * data.restSpeedCollisionMult;
                        yield return Trans(role.action.GetFlyState());
                    }
                }
            }
        }
    }

}
