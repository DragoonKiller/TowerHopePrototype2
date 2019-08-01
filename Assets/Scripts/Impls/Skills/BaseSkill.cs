using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Systems;

namespace Tower.Skills
{
    using Components;

    public interface ISkillCreator
    {
        StateMachine Create(Role role);
    }
}
