using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Global
{
    using Tower.Skills;

    [Serializable]
    public class SkillTable : MonoBehaviour
    {
        public static SkillTable inst;
        SkillTable() => inst = this;

        [SerializeField] public WindRush windRush;

    }
}
