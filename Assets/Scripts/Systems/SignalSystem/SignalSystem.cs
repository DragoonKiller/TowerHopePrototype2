using System;
using System.Collections;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 多播信号系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Signal<T>
    {
        public readonly static HashSet<Action<T>> listeners = new HashSet<Action<T>>();
        
        /// <summary>
        /// 向该信号添加一个回调.
        /// </summary>
        /// <param name="f"></param>
        public static void Listen(Action<T> f) => listeners.Add(f);

        /// <summary>
        /// 撤销该信号的一个回调.
        /// </summary>
        /// <param name="f"></param>
        public static void Remove(Action<T> f) => listeners.Remove(f);
    }

    public static class Signal
    {
        /// <summary>
        /// 发出该信号并调用函数.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        public static void Emit<T>(T v)
        {
            foreach(var i in Signal<T>.listeners) i(v);
        }
    }
}
