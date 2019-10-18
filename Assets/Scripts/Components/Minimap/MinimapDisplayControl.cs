using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tower.UI
{
    using Global;
    using Systems;
    
    [ExecuteAlways]
    public class MinimapDisplayControl : MonoBehaviour
    {
        void Update()
        {
            bool active = CommandQueue.Get(KeyBinding.inst.minimap);
            
            foreach(var i in GetComponents<MonoBehaviour>())
            {
                if(i == this) continue;
                i.enabled = active;
            }
            
            for(int i=0; i<this.transform.childCount; i++)
            {
                var x = this.transform.GetChild(i).gameObject;
                x.SetActive(active);
            }
        }
    }

}
