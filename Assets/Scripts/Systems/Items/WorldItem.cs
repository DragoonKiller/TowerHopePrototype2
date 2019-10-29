using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 放置在游戏场景中的, 可以捡起来或与角色发生交互的物品.
    /// </summary>
    public abstract class WorldItem : MonoBehaviour
    {
        [Tooltip("这个物品所属分类.")]
        public string classfifier;
        
        /// <summary>
        /// 这个物品被捡起来的回调函数. 返回应当放入物品栏的物品.
        /// 注意, 只要这个物品不是每帧都被"捡起来"一次, 这个函数的调用次数就不会很多, 返回 List 对垃圾回收的压力不大.
        /// 可行的优化方案: 先生成 classifier, 根据分发器返回的结果直接在指定背包中创建对象.
        /// </summary>
        public abstract List<(string classifier, PickedItem item)> Pick(GameObject owner);
    }
}
