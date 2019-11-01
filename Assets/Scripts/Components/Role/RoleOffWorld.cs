using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 当角色掉出指定的地图范围时, 把 hp 改 0.
    /// </summary>
    [RequireComponent(typeof(RoleHealth))]
    public sealed class RoleOffWorld : MonoBehaviour
    {
        [Tooltip("\"世界\"的坐标范围.")]
        public Rect range;
        
        RoleHealth health => this.GetComponent<RoleHealth>();
        
        void Update()
        {
            if(!range.Contains(this.transform.position.ToVec2()))
            {
                health.curHealth = 0;
            }
        }
    }
}
