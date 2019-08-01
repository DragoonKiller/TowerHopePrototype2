using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;
using Utils;

namespace Tower.Skills
{
    using Tower.Components;

    public class WindRush : ISkillCreator
    {
        public float speed;
        public float distance;
        public float restSpeedMult;

        private class STM : StateMachine
        {
            public WindRush data;
            public Role role;
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
                    role.rd.velocity = dir.normalized * data.speed;
                }
            }
        }

        public StateMachine Create(Role role) => new STM() {
            data = this,
            role = role,
        };
    }

}