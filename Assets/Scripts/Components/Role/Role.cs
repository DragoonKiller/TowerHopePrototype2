using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{

    public class Role : MonoBehaviour
    {
        public RoleAction action => this.GetComponent<RoleAction>();
        public Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
        public RoleSkills skills => this.GetComponent<RoleSkills>();
        public RoleMagic magic => this.GetComponent<RoleMagic>();
        
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
