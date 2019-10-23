using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    using Utils;
    
    public class DataReference<T> : MonoBehaviour
        where T : DataReference<T>
    {
        static readonly List<DataReference<T>> internalInst = new List<DataReference<T>>();
        public static IEnumerable<T> inst => GetEnumerator();
        DataReference<T> self;
        int index;
        
        protected DataReference() { }
        
        protected virtual void Start() 
        {
            internalInst.Add(self = this);
            index = internalInst.Count - 1;
        }
        
        protected virtual void OnDestroy()
        {
            internalInst[index] = internalInst.Last();
            internalInst.RemoveLast();
        }
        
        static IEnumerable<T> GetEnumerator() 
        {
            foreach(var i in internalInst) yield return (T)i.self;
        }
    }
}
