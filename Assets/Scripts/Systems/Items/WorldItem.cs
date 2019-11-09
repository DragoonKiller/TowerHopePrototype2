using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 放置在游戏场景中的, 以触碰的方式与玩家交互的物品.
    /// </summary>
    public abstract class WorldItem : MonoBehaviour
    {
        /// <summary>
        /// 这个物品被触碰的回调函数. 返回应当放入物品栏的物品.
        /// 注意, 只要这个物品不是每帧都被"捡起来"一次, 这个函数的调用次数就不会很多, 返回 List 对垃圾回收的压力不大.
        /// </summary>
        public abstract List<(string classifier, PickedItem item)> Touch(GameObject owner);
    }
}
