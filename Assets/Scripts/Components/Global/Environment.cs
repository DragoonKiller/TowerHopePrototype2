using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Systems;
using Utils;

namespace Tower.Components
{
    using Tower.Global;

    /// <summary>
    /// 用于各种全局控制和逻辑判定的对象.
    /// </summary>
    [ExecuteAlways]
    public class Environment : MonoBehaviour
    {
        [Tooltip("帧变速乘数. 帧时间会被乘以这个常数; 但是实际渲染流程不会发生改变.")]
        public float timeMult;

        public bool editor => Application.isEditor;
        public bool running => Application.isPlaying;
        
        void Start()
        {
            KeyBinding.inst.Load();
        }

        void Update()
        {
            CommandQueue.Create();
            CommandQueue.Run();
            StateMachine.Run();
        }

    }

}
