using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    
    public sealed class ReviveStoneWorldItem : WorldItem
    {
        [Tooltip("生成的物品栏中的复活石的模板对象.")]
        public ReviveStonePickedItem source;
        
        [Tooltip("这个物品所属分类.")]
        public string classfifier;
        
        public override List<(string classifier, PickedItem item)> Touch(GameObject owner)
        {
            var res = new List<(string classifier, PickedItem item)>();
            var pickedItem = Instantiate(source.gameObject).GetComponent<ReviveStonePickedItem>();
            res.Add((classfifier, pickedItem));
            Destroy(this.gameObject);
            return res;
        }
    }
}
