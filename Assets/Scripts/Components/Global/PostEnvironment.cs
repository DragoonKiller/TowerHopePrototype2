using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Systems;
using Utils;

namespace Tower.Components
{
    using Tower.Global;

    /// <summary>
    /// 用于各种全局控制和逻辑判定的对象.
    /// </summary>
    [ExecuteAlways]
    public class PostEnvironment : MonoBehaviour
    {
        public bool editor => Application.isEditor;
        public bool running => Application.isPlaying;
        
        void Update()
        {
            
            CommandQueue.Clear();
        }

    }

}
