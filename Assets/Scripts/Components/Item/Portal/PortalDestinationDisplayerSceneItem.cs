using System;
using System.Collections.Generic;
using UnityEngine;


namespace Tower.Components
{
    using Systems;

    /// <summary>
    /// 当主角与该物体的碰撞盒相交时, 显示传送门的目标地点.
    /// </summary>
    [ExecuteAlways]
    public class PortalDestinationDisplayerSceneItem : SceneItem
    {
        [Tooltip("绑定的传送门.")]
        public PortalInteractionItem portal;
        
        [Tooltip("绑定的目标设置物件.")]
        public PortalDestinationSelectorInteractionItem selection;
        
        [Tooltip("绑定的文本.")]
        public TextMesh text;
        
        [Header("Debug")]
        
        [Tooltip("与该物体交互的主角.")]
        public List<GameObject> touching;
        
        void Update()
        {
            if(touching.Count != 0)
            {
                text.text = selection.currentSelection;
            }
            else
            {
                text.text = "";
            }
        }
        
        void OnTriggerEnter2D(Collider2D c) => TryAdd(c.gameObject);
        
        void OnCollisionEnter2D(Collision2D c) => TryAdd(c.gameObject);
        
        void OnTriggerExit2D(Collider2D c) => TryRemove(c.gameObject);
        
        void OnCollisionExit2D(Collision2D c) => TryRemove(c.gameObject);
        
        void TryAdd(GameObject t)
        {
            if(!t.TryGetComponent<RolePlayerControl>(out _)) return;
            touching.Add(t);
        }
        
        void TryRemove(GameObject t)
        {
            if(!t.TryGetComponent<RolePlayerControl>(out _)) return;
            touching.Remove(t);
        }
        
    }

}
