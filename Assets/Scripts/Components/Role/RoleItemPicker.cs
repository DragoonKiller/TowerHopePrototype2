using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Tower.Components
{
    using Utils;
    using Systems;
    
    /// <summary>
    /// 用于控制角色拾取物品.
    /// </summary>
    public class RoleItemPicker : MonoBehaviour
    {
        public ItemDispatcher dispatcher;
        
        void OnTriggerEnter2D(Collider2D c)
        {
            if(!c.gameObject.TryGetComponent<WorldItem>(out var worldItem)) return;
            
            // 生成分类器和物品.
            var items = worldItem.Pick(this.gameObject);
            
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
