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
        [Tooltip("该脚本管理的尾拖轨迹.")]
        public List<TrailRenderer> trail;
        
        /// <summary>
        /// 取消所有尾拖显示.
        /// </summary>
        public void Deactive()
        {
            foreach(var i in trail) i.enabled = false;
        }
        
        /// <summary>
        /// 开启所有尾拖显示.
        /// </summary>
        public void Active()
        {
            foreach(var i in trail) i.enabled = true;
        }
        
        /// <summary>
        /// 清除当前的所有尾拖. 尾拖特效将从新的位置重新开始.
        /// </summary>
        public void Reset()
        {
            foreach(var i in trail) i.Clear();
        }
    }
}
