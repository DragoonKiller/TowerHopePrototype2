using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Tower.Skills;

    public class RoleSkills : MonoBehaviour
    {
        // 下面是各种技能.

        public ISkillCreator rushCreator;
        public ISkillCreator primarySkillCreator;
        public ISkillCreator secondarySkillCreator;
        public ISkillCreator attackCreator;
        public ISkillCreator magicAttackCreator;

        public Role role => this.GetComponent<Role>();

        void Start()
        {
            rushCreator = new WindRush() {
                speed = 24f,
                distance = 4f,
                restSpeedMult = 0.2f,
            };
        }
    }

}