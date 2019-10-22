using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;
    
    public class AimingArrow : MonoBehaviour
    {
        public Transform target;
        
        void Update()
        {
            var fromPos = target.position.ToVec2();
            var toPos = ExCursor.worldPos;
            var dir = fromPos.To(toPos);
            var angle = dir.Angle();
            this.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
    }

}
