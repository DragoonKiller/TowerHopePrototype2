﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Systems
{
    /// <summary>
    /// 支持分层状态机的各种操作.
    /// </summary>
    public abstract class StateMachine
    {
        // ============================================================================================================
        // Classes and structs
        // ============================================================================================================

        /// <summary>
        /// 表示状态机内部或之间的一次转移过程.
        /// </summary>
        public struct Transfer
        {
            /// <summary>
            /// 表示该转移的转移方式.
            /// </summary>
            public enum Type
            {
                /// <summary>
                /// 仅仅把该状态机放到队列最后, 而不执行任何其它的操作.
                /// 通常是内部使用, 用于处理传入的状态机失效的情况.
                /// </summary>
                Yield,

                /// <summary>
                /// 该帧结束, 执行下一帧.
                /// </summary>
                Pass,

                /// <summary>
                /// 层次调用下一个状态机.
                /// </summary>
                Call,

                /// <summary>
                /// 额外调用一个状态机.
                /// </summary>
                CallPass,

                /// <summary>
                /// 停止当前状态机(包括其父节点)的运行, 并调用另一个状态机.
                /// </summary>
                Transfer,
            }

            /// <summary>
            /// 该转移所表示的转移方式.
            /// </summary>
            public Type type;

            /// <summary>
            /// 接下来要操作的状态机.
            /// </summary>
            public StateMachine next;
        }

        /// <summary>
        /// 用于唯一地标记一个状态机.
        /// 状态机可能会在不同的 StateMachine class 中转移, 
        /// 但是这个标记不会发生改变.
        /// 这样即使换了一个状态机对象也可以从外部删除.
        /// </summary>
        public class Tag { }

        // ============================================================================================================
        // Dynamic storage and maintaince
        // ============================================================================================================

        /// <summary>
        /// 每个状态机实例会保存自己的当前状态.
        /// </summary>
        IEnumerator<Transfer> curState = null;

        /// <summary>
        /// 它是从哪个状态机调用的. <br/>
        /// 通过 Transfer.Type.Call 进行层次调用时,
        /// 需要该字记录应当返回哪个状态. <br/>
        /// </summary>
        StateMachine parent = null;

        /// <summary>
        /// 状态机的唯一编号.
        /// </summary>
        public Tag tag;
        
        /// <summary>
        /// 指示这个状态机是否还在活动.
        /// </summary>
        public bool active = true;
        
        /// <summary>
        /// 状态机每帧执行逻辑.
        /// </summary>
        public abstract IEnumerator<Transfer> Step();
        
        /// <summary>
        /// 新建一个状态机.
        /// </summary>
        public StateMachine()
        {
            tag = new Tag();
        }
        
        /// <summary>
        /// 继承创建一个状态机.
        /// </summary>
        StateMachine(StateMachine sm)
        {
            this.tag = sm.tag;
        }
        
        /// <summary>
        /// 复制给定状态机的tag到这个状态机中.
        /// </summary>
        StateMachine Inheritance(StateMachine sm)
        {
            this.tag = sm.tag;
            return this;
        }

        // ============================================================================================================
        // Static storage and maintance
        // ============================================================================================================

        /// <summary>
        /// 状态机的两个执行队列中的第一个.
        /// </summary>
        readonly static Queue<StateMachine> queueA = new Queue<StateMachine>();

        /// <summary>
        /// 状态机的两个执行队列中的第二个.
        /// </summary>
        readonly static Queue<StateMachine> queueB = new Queue<StateMachine>();

        /// <summary>
        /// 待删除的状态机列表.
        /// </summary>
        readonly static HashSet<Tag> removeList = new HashSet<Tag>();
 
        /// <summary>
        /// 存两个队列.
        /// </summary>
        readonly static Queue<StateMachine>[] queues;

        /// <summary>
        /// 当前队列编号.
        /// </summary>
        static int curQueue;

        /// <summary>
        /// 这个函数在类被加载时运行.
        /// </summary>
        static StateMachine()
        {
            queues = new Queue<StateMachine>[2] { queueA, queueB };
        }

        /// <summary>
        /// 示意该帧已结束, 等待下一帧.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transfer Pass() => new Transfer() { type = Transfer.Type.Pass, next = null };
        
        /// <summary>
        /// 层次调用新的状态, 新的状态结束后会返回调用处.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transfer Call(StateMachine x) => new Transfer() {
            type = x == null ? Transfer.Type.Yield : Transfer.Type.Call,
            next = x == null ? null : x.Inheritance(this)
        };
        
        /// <summary>
        /// 额外调用一个状态机而不影响自身的执行.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transfer CallPass(StateMachine x) => new Transfer() {
            type = x == null ? Transfer.Type.Yield : Transfer.Type.CallPass,
            next = x == null ? null : x.Inheritance(this)
        };
        
        /// <summary>
        /// 切换到一个新的状态, 不保留原状态.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Transfer Trans(StateMachine x) => new Transfer() {
            type = x == null ? Transfer.Type.Yield : Transfer.Type.Transfer,
            next = x == null ? null : x.Inheritance(this)
        };
        
        /// <summary>
        /// 把状态机注册到全局维护队列中.
        /// </summary>
        public static T Register<T>(T stateMachine) where T : StateMachine
        {
            queues[curQueue].Enqueue(stateMachine);
            return stateMachine;
        }
        
        /// <summary>
        /// 将该 tag 指向的状态机标记为"待删除".
        /// </summary>
        public static void Remove(Tag tag) => removeList.Add(tag);
        
        /// <summary>
        /// 删除所有被标记为 "待删除" 的状态机.
        /// 让所有状态机往前推进一帧.
        /// </summary>
        public static void Run()
        {
            ClearRemove();

            var cur = queues[curQueue];
            var nxt = queues[curQueue ^ 1];
            curQueue ^= 1;

            while(cur.Count != 0)
            {
                var x = cur.Dequeue();
                
                // x 不活动. 把它放到下一帧.
                if(!x.active)
                {
                    nxt.Enqueue(x);
                    continue;
                }
                
                // x 是第一次开始执行
                if(x.curState == null) x.curState = x.Step();
                
                var curState = x.curState;
                var processSuccess = curState.MoveNext();

                if(processSuccess)
                {
                    // MoveNext() 之后的迭代器会发生改变.
                    // 但是该迭代器可能是值类型.
                    // 所以要重新赋值.
                    x.curState = curState;

                    var trans = curState.Current;

                    switch(trans.type)
                    {
                    case Transfer.Type.Yield:
                    {
                        // Yield 操作, 将 x 放到队列末尾.
                        cur.Enqueue(x);
                        break;
                    }

                    case Transfer.Type.Pass:
                    {
                        // Step 操作.
                        // 将 x 放入另一个队列.
                        nxt.Enqueue(x);
                        break;
                    }

                    case Transfer.Type.Call:
                    {
                        // Call 操作
                        // 将 x 拿出队列, 将 x 的引用存入next.
                        // 将 next 放入当前队列.
                        var next = trans.next;
                        cur.Enqueue(next);
                        next.parent = x;
                        break;
                    }

                    case Transfer.Type.CallPass:
                    {
                        // CallPass 操作
                        // 将 x 的引用存入 next.
                        // 将 x 和 next 放入队列等待执行.
                        var next = trans.next;
                        cur.Enqueue(next);
                        cur.Enqueue(x);
                        next.parent = x;
                        break;
                    }

                    case Transfer.Type.Transfer:
                    {
                        // Transfer 操作
                        // 将 x 从队列移除.
                        // 将 next 放到当前队列执行.
                        var next = trans.next;
                        cur.Enqueue(next);
                        break;
                    }

                    default: throw new NotSupportedException();
                    }
                }
                else // 对应 yield break, 状态机执行已经结束.
                {
                    // 需要返回到上一层状态机.
                    if(x.parent != null)
                    {
                        // 注意不是放到当前状态机队列.
                        nxt.Enqueue(x.parent);
                    }
                }
            }
        }

        /// <summary>
        /// 删除所有被主动标记为"待删除"的正在执行的状态机.
        /// 不会理会那些不存在于队列中, 但是标记为待删除的状态机.
        /// </summary>
        static void ClearRemove()
        {
            var cur = queues[curQueue];
            int cnt = cur.Count;
            for(int i = 0; i < cnt; i++)
            {
                var x = cur.Dequeue();
                if(!removeList.Contains(x.tag)) cur.Enqueue(x);
            }
            removeList.Clear();
        }

        // ============================================================================================================
        // UnitTest
        // ============================================================================================================




    }
}
