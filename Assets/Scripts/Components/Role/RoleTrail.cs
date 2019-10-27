using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 控制角色的尾拖特效.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleTrail : MonoBehaviour
    {
        public List<TrailRenderer> trail;
        
        void Update()
        {
            
        }
    }
}
