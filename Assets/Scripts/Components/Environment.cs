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
        public struct SceneStart { };
    }
}


namespace Tower.Components
{
    using Tower.Global;

    /// <summary>
    /// 用于各种全局控制和逻辑判定的对象.
    /// </summary>
    public class Environment : MonoBehaviour
    {
        // public KeyBinding keyBinding;

        [SerializeField] bool started = false;

        void Start()
        {
            Signal<Signals.SceneStart>.Listen((x) => 
            {
                KeyBinding.inst.Load();
            });

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
            if(!started)
            {
                Signal.Emit(new Signals.SceneStart());
                started = true;
            }
            Signal.Emit(new Signals.RenderUpdate() { dt = Time.deltaTime });
        }

        void FixedUpdate()
        {
            Signal.Emit(new Signals.PhysicsUpdate() { dt = Time.fixedDeltaTime });
        }

    }

}