using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 放置在游戏场景中的, 由玩家主动产生交互的物品.
    /// 交互效果由继承自该脚本的脚本控制.
    /// "交互提示" 由其它脚本控制.
    /// 这个脚本可以用来做一些简单的 NPC.
    /// 和 Role 系列脚本组合可以创造足够复杂的交互 NPC.
    /// </summary>
    public abstract class InteractionItem : MonoBehaviour
    {
        /// <summary>
        /// 主动与对象交互的回调函数.
        /// </summary>
        public abstract void Interact(GameObject op);
    }
}
