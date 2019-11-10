using System;
using System.Collections.Generic;
using UnityEngine;


namespace Tower.Components
{
    using Systems;
    using Utils;
    
    /// <summary>
    /// 处理传送门的目的地.
    /// </summary>
    [ExecuteAlways]
    public class PortalDestinationSelectorInteractionItem : InteractionItem
    {
        [Tooltip("对应的传送门.")]
        public PortalInteractionItem portal;
        
        [Tooltip("有哪些传送目标选项.")]
        public List<string> targetScenes;
        
        [Tooltip("当前选项.")]
        public int currentSelectionId;
        
        public string currentSelection => targetScenes[currentSelectionId];
        
        void Update()
        {
            portal.targetSceneName = currentSelection;
        }
        
        public override void Interact(GameObject op)
        {
            currentSelectionId = (currentSelectionId + 1).ModSys(targetScenes.Count);
        }
    }

}
