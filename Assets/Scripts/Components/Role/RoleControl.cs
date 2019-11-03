using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Tower.Components
{
    using Systems;
    using Utils;
    
    /// <summary>
    /// 安排和决定这个物体应该由哪个脚本来控制.
    /// 一个物体可能同时有多个控制脚本与之关联, 但是只有其中一个能正确运行.
    /// 例如, 怪物自己有一个AI, 同时有一个玩家技能可以把怪物拖向玩家.
    /// 注意, 怪物的控制器可能有很多., 有若干解决方案:
    /// 1. 写一个统一的调度器. 控制脚本向调度器注册; 调度器决定应该使用哪个控制脚本.
    /// 2. 
    /// </summary>
    public sealed class RoleControl : MonoBehaviour
    {
        
        
    }
}
