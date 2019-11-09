using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Systems
{
    /// <summary>
    /// 存储一些预定义的通用的状态机.
    /// 有如下约定:
    /// * StateMachine.Run() 应当随着时间推进而不断地执行.
    /// * StateMachine.Run() 应当每帧执行一次.
    /// </summary>
    public static class DefaultStateMachine
    {
        /// <summary>
        /// 等待若干秒. 当等待时长大于等于给定数值时, 状态机结束运行.
        /// </summary>
        public sealed class WaitForSeconds : StateMachine
        {
            public float time;
            
            public WaitForSeconds(float t) => time = t;

            public override IEnumerator<Transfer> Step()
            {
                var beginTime = Time.time;
                while(Time.time - beginTime >= time)
                {
                    yield return Pass();
                }
            }
        }
        
        /// <summary>
        /// 等待若干帧. 当状态机存在的帧数大于等于给定数值时, 状态机结束运行.
        /// </summary>
        public sealed class WaitForFrames : StateMachine
        {
            public int time;
            
            public WaitForFrames(int t) => time = t;

            public override IEnumerator<Transfer> Step()
            {
                var x = 0;
                while(x < time)
                {
                    yield return Pass();
                    x++;
                }
            }
        }
    }
}
