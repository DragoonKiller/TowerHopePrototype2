using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Skills
{
    using Tower.Components;
    using Systems;

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
        
        [Tooltip("冲刺尾部的爆炸特效对象.")]
        public GameObject localExplode;
        
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
                
                var fx = GameObject.Instantiate(data.localExplode, role.transform.position, Quaternion.identity);
                
                while(true)
                {
                    // 如果超时了, 把速度设置到基础速度的一个倍数, 然后退出.
                    if(Time.time - beginTime >= data.duration)
                    {
                        role.rd.velocity = dir.normalized * data.speed * data.restSpeedMult;
                        yield return Trans(role.action.GetFlyState());
                    }
                    
                    yield return Pass();
                    
                    // 处理速度.
                    var rate = (Time.time - beginTime) / data.duration;
                    var nextV = data.curve.Evaluate(rate) * data.speed * dir.normalized;
                    role.rd.velocity = nextV;
                    
                    // 绘制移动范围.
                    DebugDraw.Circle(role.transform.position, data.curve.Integral(rate, 1.0f) * data.speed * data.duration, Color.red);
                    
                    // 检测是否撞到了地形.
                    var ct = role.rd.GetContacts(new ContactFilter2D() {
                        layerMask = LayerMask.GetMask("Terrain")
                    }, res);
                    
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
