using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Systems;
using Utils;

namespace Systems
{
    public static partial class Signals
    {
        public struct PreUpdate { public float dt; }
        public struct PrePhysicsUpdate { public float dt; }
        public struct PostPhysicsUpdate { public float dt; }
        public struct PostUpdate { public float dt; }

        /// <summary>
        /// Environment 脚本被加载时的事件.
        /// </summary>
        public struct SceneStart { };
    }
}


namespace Tower.Components
{
    using Tower.Global;

    /// <summary>
    /// 用于各种全局控制和逻辑判定的对象.
    /// </summary>
    [ExecuteAlways]
    public class Environment : MonoBehaviour
    {
        [NonSerialized] public Environment inst;

        // 物理帧计时器.
        float physicsTimer;

        [Tooltip("帧变速乘数. 帧时间会被乘以这个常数; 但是实际渲染流程不会发生改变.")]
        public float timeMult;

        public bool editor => Application.isEditor;
        public bool running => Application.isPlaying;

        void Start()
        {
            KeyBinding.inst.Load();
            physicsTimer = Time.fixedTime;
        }

        void Update()
        {
            CommandQueue.Create();
            Signal.Emit(new Signals.PreUpdate() { dt = Time.deltaTime * timeMult });
            CommandQueue.Run();
            
            Signal.Emit(new Signals.PrePhysicsUpdate() { dt = Time.deltaTime * timeMult });
            Physics2D.Simulate(Time.deltaTime * timeMult);
            Signal.Emit(new Signals.PostPhysicsUpdate() { dt = Time.deltaTime * timeMult });

            StateMachine.Run();
            Signal.Emit(new Signals.PostUpdate() { dt = Time.deltaTime * timeMult });
            CommandQueue.Clear();
        }

    }

}