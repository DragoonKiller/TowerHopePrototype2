using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Tower.Components
{
    using Tower.Global;
    using Utils;
    using Systems;
    
    /// <summary>
    /// 设置角色与物件的主动交互.
    /// 角色与物体接触时, 会由该脚本控制"可以主动交互"的提示,
    /// 并处理角色输入, 判断是否开始交互.
    /// 具体的交互操作由物体控制.
    /// </summary>
    public class RoleInteraction : MonoBehaviour
    {
        [Tooltip("当角色可以与物品交互时, 指示交互物品的对象.")]
        public GameObject hint;
        
        [Header("Debug")]
        
        [Tooltip("当前有多少个物品可以交互.")]
        public int currentItemsCount;
        
        
        [Tooltip("当前可以交互的物品. 从中选择最后添加的一个进行交互.")]
        readonly List<InteractionItem> currentItems = new List<InteractionItem>();
        
        
        void Update()
        {
            currentItemsCount = currentItems.Count;
            AdjustHint();
            TryInteraction(); 
        }
        
        void AdjustHint()
        {
            if(!hint) return;
            
            if(currentItems.Count != 0)
            {
                hint.SetActive(true);
                hint.transform.position = currentItems.Last().transform.position;
            }
            else
            {
                hint.SetActive(false);
            }
        }
        
        void TryInteraction()
        {
            if(!CommandQueue.Get(KeyBinding.inst.interact)) return;
            if(currentItems.Count == 0) return;
            
            currentItems.Last().Interact(this.gameObject);
        }
        
        void OnTriggerEnter2D(Collider2D c) => TryAddItem(c.gameObject);
        
        void OnCollisionEnter2D(Collision2D c) => TryAddItem(c.gameObject);
        
        void TryAddItem(GameObject c)
        {
            if(!c.TryGetComponent<InteractionItem>(out var item)) return;
            currentItems.Add(item);
        }
        
        void OnTriggerExit2D(Collider2D c) => TryRemoveItem(c.gameObject);
        
        void OnCollisionExit2D(Collision2D c) => TryRemoveItem(c.gameObject);
        
        void TryRemoveItem(GameObject c)
        {
            if(!c.TryGetComponent<InteractionItem>(out var item)) return;
            currentItems.Remove(item);
        }
    }
}
