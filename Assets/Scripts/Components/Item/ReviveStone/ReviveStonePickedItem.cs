using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Utils;
    using Systems;
    
    public sealed class ReviveStonePickedItem : PickedItem
    {
        public override void Consume()
        {
            DestroyImmediate(this.gameObject);
        }
        
        public override void Throw()
        {
            
        }
    }
}
