using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Skills
{
    using Tower.Components;

    public class WindRush : ISkillConfig
    {
        public float preserveTime;
        public float magicCost;
        public float speed;
        public float distance;
        public float restSpeedMult;
        public float restSpeedCollisionMult;
        
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
                dir.Len(dir.magnitude.Min(data.distance));
                var duration = data.distance / data.speed;
                var beginTime = Time.time;
                Debug.DrawLine(from, from + dir);
                while(true)
                {
                    if(Time.time - beginTime >= duration)
                    {
                        role.rd.velocity = dir.normalized * data.speed * data.restSpeedMult;
                        yield break;
                    }
                    yield return Pass();
                    var ct = role.rd.GetContacts(new ContactFilter2D() {
                        layerMask = LayerMask.GetMask("Terrain")
                    }, res);

                    
                    var nextV = dir.normalized * data.speed ;
                    role.rd.velocity = nextV;

                    // 在该时限之外, 如果撞到了东西, 结束...
                    if(Time.time - beginTime > data.preserveTime && ct != 0)
                    {
                        role.rd.velocity = dir.normalized * data.speed * data.restSpeedCollisionMult;
                        yield break;
                    }

                }
            }
        }

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
    }

}