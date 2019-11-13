using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Tower.Global;
    using Tower.Skills;
    
    /// <summary>
    /// 主角的一个快速访问的标记.
    /// </summary>
    public sealed class RoleProtagonist : MonoBehaviour
    {
        readonly static List<RoleProtagonist> _inst = new List<RoleProtagonist>();
        public static RoleProtagonist inst => _inst[0];
        void Awake() => _inst.Add(this);
        void OnDestroy() => _inst.Remove(this);
    }
}
