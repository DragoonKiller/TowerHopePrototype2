using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tower.Components
{
    using Systems;
    using Utils;

    /// <summary>
    /// 用于存储生命值相关信息, 控制角色的死亡状态和复活.
    /// 注意: 角色的死亡动画由 RoleAction 或者别的什么东西完成. 反正不由此负责.
    /// RoleAction 或者其他脚本应当调用该脚本查询死亡状态.
    /// </summary>
    [RequireComponent(typeof(Role))]
    public sealed class RoleHealth : MonoBehaviour
    {
        
        void Update()
        {
            
        }
    }
}
