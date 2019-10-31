using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 当角色掉出地图时(坐标值大于等于10000), 把 hp 改 0.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleOffWorld : MonoBehaviour
    {
        [Tooltip("\"世界\"的坐标范围.")]
        public Rect range;
     
        Role role => this.GetComponent<Role>();
           
        void Update()
        {
            if(!range.Contains(this.transform.position.ToVec2()))
            {
                role.health.curHealth = 0;
            }
        }
    }
}
