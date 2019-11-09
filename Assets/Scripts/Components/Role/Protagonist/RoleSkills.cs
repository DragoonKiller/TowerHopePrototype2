using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Tower.Global;
    using Tower.Skills;

    public sealed class RoleSkills : MonoBehaviour
    {
        // 下面是各种技能.
        public ISkillConfig rush;
        public ISkillConfig primary;
        public ISkillConfig secondary;
        public ISkillConfig attack;
        public ISkillConfig magicAttack;
        
        void Start()
        {
            rush = SkillTable.inst.windRush;
            attack = SkillTable.inst.sampleAttack;
        }
    }
}
