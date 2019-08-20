using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Systems
{
    public static partial class Signals
    {
        public struct Attack { public int phase; }
    }
}

namespace Tower.Animation
{
    using Systems;


    public class SampleAttackAnimation : MonoBehaviour
    {
        public class AttackPhase
        {

        }

        void Start()
        {
            Signal<Signals.PostUpdate>.Listen(Step);
        }

        void OnDestroy()
        {
            Signal<Signals.PostUpdate>.Remove(Step);
        }

        void Step(Signals.PostUpdate e)
        {
            
        }

    }

}