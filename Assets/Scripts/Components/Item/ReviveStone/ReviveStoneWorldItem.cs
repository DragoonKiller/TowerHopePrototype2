using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    
    public sealed class ReviveStoneWorldItem : WorldItem
    {
        public ReviveStonePickedItem source;
        
        public override List<(string classifier, PickedItem item)> Pick(GameObject owner)
        {
            var res = new List<(string classifier, PickedItem item)>();
            var pickedItem = Instantiate(source.gameObject).GetComponent<ReviveStonePickedItem>();
            res.Add((classfifier, pickedItem));
            Destroy(this.gameObject);
            return res;
        }
    }
}
