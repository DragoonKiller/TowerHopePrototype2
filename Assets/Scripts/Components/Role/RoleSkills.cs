using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Tower.Skills;

    [RequireComponent(typeof(Role))]
    public sealed class RoleSkills : MonoBehaviour
    {
        // 下面是各种技能.

        public ISkillConfig rush;
        public ISkillConfig primary;
        public ISkillConfig secondary;
        public ISkillConfig attack;
        public ISkillConfig magicAttack;

        public Role role => this.GetComponent<Role>();

        void Start()
        {
            rush = new WindRush() {
                magicCost = 3f,
                preserveTime = 0.1f,
                speed = 24f,
                distance = 4f,
                restSpeedMult = 0.2f,
                restSpeedCollisionMult = 0.1f,
            };
        }
    }

}