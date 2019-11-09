using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Tower.Components
{
    using Utils;
    using Systems;
    
    /// <summary>
    /// 用于控制角色"被动地"拾取物品.
    /// 只要碰到就算作被拾取, 直接查找是否有相关的组件和函数.
    /// </summary>
    public class RoleItemPicker : MonoBehaviour
    {
        public ItemDispatcher dispatcher;
        
        void OnTriggerEnter2D(Collider2D c) => DealWithCollision(c);
        
        void OnCollisionEnter2D(Collision2D c) => DealWithCollision(c.collider);
        
        void DealWithCollision(Collider2D c)
        {
            if(!c.gameObject.TryGetComponent<WorldItem>(out var worldItem)) return;
            
            // 生成分类器和物品.
            var items = worldItem.Touch(this.gameObject);
            
            foreach(var (cls, item) in items)
            {
                var ivt = dispatcher.GetInventory(cls);
                
                // 如果没有合适的放置这个物品的地方, 就直接把它删掉.
                if(ivt == null)
                {
                    DestroyImmediate(item.gameObject);
                    continue;
                }
                
                // 把物品放入背包中.
                item.transform.SetParent(ivt);
                item.transform.Clear();
            }
        }
    }
}
