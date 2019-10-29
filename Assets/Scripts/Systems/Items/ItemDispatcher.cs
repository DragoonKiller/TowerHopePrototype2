using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 物品分发器. 这个类决定了哪些物品应当放到哪个背包中.
    /// 注意, 这个类仅仅作为筛选器使用.
    /// 它并不会从逻辑上限制调用者把其它类型的物品放入背包.
    /// </summary>
    [Serializable]
    public sealed class ItemDispatcher
    {
        /// <summary>
        /// 分发规则.
        /// </summary>
        public enum DispatchRule
        {
            /// <summary>
            /// 如果物品的 classifier 出现在背包的 classifier 列表中, 这个背包就接收这个物品.
            /// </summary>
            Include,
            
            /// <summary>
            /// 如果物品的 classifier 没有出现在背包的 classifier 列表中, 这个背包就接收这个物品.
            /// </summary>
            Exclude,
        }
        
        /// <summary>
        /// 指定一个背包.
        /// </summary>
        [Serializable]
        public struct Inventory
        {
            [Tooltip("背包对应的游戏对象.")]
            public Transform inventory;
            
            [Tooltip("分发规则.")]
            public DispatchRule rule;
            
            [Tooltip("这个背包的判断条件包含哪些 classifier.")]
            public List<string> classifiers;
        }
        
        /// <summary>
        /// 这个分发器管辖的所有背包, 以及它们的分发方式.
        /// </summary>
        public List<Inventory> inventories;
        
        /// <summary>
        /// 按照设置好的规则, 获取一个物品对应的背包. 如果找不到, 返回 null.
        /// </summary>
        public Transform GetInventory(string classifier)
        {
            foreach(var ivt in inventories)
            {
                if(ivt.rule == DispatchRule.Include)
                {
                    if(ivt.classifiers.Contains(classifier)) return ivt.inventory;
                }
                else if(ivt.rule == DispatchRule.Exclude)
                {
                    if(!ivt.classifiers.Contains(classifier)) return ivt.inventory;
                }
            }
            
            return null;
        }
    }
}
