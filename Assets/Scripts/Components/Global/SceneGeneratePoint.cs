using System;
using UnityEngine;

namespace Tower.Components
{
    using Utils;
    
    public class SceneGeneratePoint : MonoBehaviour
    {
        public static SceneGeneratePoint inst;
        
        void Awake() => inst = this;
        
        void Start()
        {
            inst = null;
            var target = RoleProtagonist.inst;
            target.transform.position = target.transform.position.XY(this.transform.position);
            Destroy(this);
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(this.transform.position, Vector3.one * 1);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, 2.Sqrt() / 2);
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(this.transform.position, Vector3.one * 2.Sqrt());
        }
    }
    
}
