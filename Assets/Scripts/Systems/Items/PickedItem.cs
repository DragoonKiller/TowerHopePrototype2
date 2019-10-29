using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    
    /// <summary>
    /// 放置在背包里的物品.
    /// </summary>
    public abstract class PickedItem : MonoBehaviour
    {
        [Tooltip("物品描述.")]
        public string description;
        
        [Tooltip("物品拥有者.")]
        public GameObject owner;
        
        /// <summary>
        /// 丢弃物品的回调函数.
        /// </summary>
        public abstract void Throw();
        
        /// <summary>
        /// 使用物品的回调函数.
        /// </summary>
        public abstract void Consume();
    }
}
