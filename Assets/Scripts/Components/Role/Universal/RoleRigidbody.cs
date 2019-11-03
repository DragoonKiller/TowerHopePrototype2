using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class RoleRigidbody : MonoBehaviour
    {
        public Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
        
        public void Start()
        {
            rd.simulated = true;
        }
        
        public void OnDestroy()
        {
            rd.simulated = false;
        }
    }
}
