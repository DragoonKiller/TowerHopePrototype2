using System;
using System.Collections.Generic;

namespace Tower.Systems
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
                /// 该帧结束, 执行下一帧.
                /// </summary>
                Step,

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

            public static Transfer Step() => new Transfer() { type = Type.Step, next = null };
            public static Transfer Call(StateMachine x) => new Transfer() { type = Type.Call, next = x };
            public static Transfer CallPass(StateMachine x) => new Transfer() { type = Type.CallPass, next = x };
            public static Transfer Trans(StateMachine x) => new Transfer() { type = Type.Transfer, next = x };
        }

        public abstract IEnumerator<Transfer> Step();

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
        /// 每构造一个状态机, 它在下一帧(下一次运行 StateMachine.Run )就会直接运行起来. <br>
        /// 可以通过在 Step 函数里写入一些等待的代码.
        /// </summary>
        public StateMachine()
        {
            queues[curQueue].Enqueue(this);
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
        /// 让所有状态机往前推进一帧.
        /// </summary>
        /// <param name="x"></param>
        public static void Run()
        {
            var cur = queues[curQueue];
            var nxt = queues[curQueue ^ 1];
            curQueue ^= 1;

            while(cur.Count != 0)
            {
                var x = cur.Dequeue();

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
                    case Transfer.Type.Step:
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

        // ============================================================================================================
        // UnitTest
        // ============================================================================================================




    }
}
