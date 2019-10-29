using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;

namespace Tower.Skills
{
    using Components;
    
    public interface ISkillConfig
    {
        bool TryGetState(Role role, out StateMachine stateMachine);
    }
}
