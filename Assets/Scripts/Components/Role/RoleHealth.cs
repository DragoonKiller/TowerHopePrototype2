using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 用于存储生命值相关信息, 控制角色的生命值, 报告其生命值状态.
    /// 注意: 角色的死亡动画, 复活等, 由 RoleAction 或者别的什么东西完成. 不由此脚本负责.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleHealth : MonoBehaviour
    {
        [Tooltip("最大生命值.")]
        public float maxHealth;
        
        [Tooltip("当前生命值.")]
        public float curHealth;
        
        [Tooltip("基础生命恢复速率.")]
        public float healthRecover;
        
        /// <summary>
        /// 角色是否处于死亡状态.
        /// </summary>
        public bool dead { get; private set; }
        
        void Update()
        {
            curHealth = curHealth.Max(0);
            dead = curHealth == 0;
            curHealth = (curHealth + healthRecover * Time.deltaTime).Min(maxHealth);
        }
    }
}
