using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static partial class Signals
    {
        public struct MagicSystemUpdate { }
    }
}


namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 用于存储各种与当前法力值相关的信息, 并维护当前的法力值不要超过下界.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleMagic : MonoBehaviour
    {
        [Tooltip("法力值.")]
        public float magic;

        [Tooltip("最大法力值.")]
        public float maxMagic;

        [Tooltip("环境因素影响的每秒法力回复值.")]
        public float recoverRate;
        
        public Role role => this.GetComponent<Role>();
        
        public bool TryUseMagic(float amount)
        {
            if(magic >= amount)
            {
                magic -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 回复给定值的法力.
        /// </summary>
        public bool RecoverMagic(float amount)
        {
            if(amount < 0f) return false;
            magic += amount;
            magic = magic.Clamp(0f, maxMagic);
            return true;
        }

        void Update()
        {
            if(role.action == null || (role.action.touchingGround || role.action.touchingWall))
            {
                RecoverMagic(Time.deltaTime * recoverRate);
            }
        }
    }
}
