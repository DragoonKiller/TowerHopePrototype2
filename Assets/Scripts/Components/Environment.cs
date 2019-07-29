using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Systems;
using Utils;

namespace Systems
{
    public static partial class Signals
    {
        public struct RenderUpdate { public float dt; }
        public struct PhysicsUpdate { public float dt; }
    }
}


namespace Tower.Components
{
    /// <summary>
    /// 用于各种全局控制和逻辑判定的对象.
    /// </summary>
    public class Environment : MonoBehaviour
    {
        void Start()
        {
            Signal<Signals.RenderUpdate>.Listen((x) =>
            {
                CommandQueue.Create();
                CommandQueue.Run();
                StateMachine.Run();
                CommandQueue.Clear();
            });
        }

        void Update()
        {
            Signal.Emit(new Signals.RenderUpdate() { dt = Time.deltaTime });
        }

        void FixedUpdate()
        {
            Signal.Emit(new Signals.PhysicsUpdate() { dt = Time.fixedDeltaTime });
        }

    }

}