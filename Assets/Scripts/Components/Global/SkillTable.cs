using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Global
{
    using Tower.Skills;

    [Serializable]
    [CreateAssetMenu(fileName = "SkillTable", menuName = "Global/SkillTable", order = 10)]
    public class SkillTable : ScriptableObject
    {
        public static SkillTable inst;
        SkillTable() => inst = this;

        [SerializeField] public SampleAttack sampleAttack;
        [SerializeField] public WindRush windRush;

    }
}
