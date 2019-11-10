using System;
using UnityEngine;


namespace Tower.Components
{
    using Systems;

    /// <summary>
    /// 处理传送门和主角的交互.
    /// </summary>
    public class PortalInteractionItem : InteractionItem
    {
        [Tooltip("目标场景.")]
        public string targetSceneName;
        
        public override void Interact(GameObject op)
        {
            // TODO
            Debug.Log("Interact");
        }
    }
}
