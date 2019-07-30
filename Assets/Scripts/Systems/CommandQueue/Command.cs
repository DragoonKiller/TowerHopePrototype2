using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Utils;

namespace Systems
{
    // 
    // 指令系统 
    // 
    // 作为玩家输入和角色动作的中间层, 玩家的输入和指令绑定.
    // 每个玩家输入都会向指令队列添加一条指令.
    // 玩家的每一个输入都对应 0 到 1 条指令.
    // control 和 shift 按键是修饰键, 因此完整的按键应当是 (key, ctrl, shift)
    // 如果同时绑定了修饰键和非修饰键, 例如 (A, false, true) 和 (A, false, false)
    // 那么带有修饰键的命令将会产生, 而非修饰键指令不会产生.
    // 
    // 状态机等可以通过该指令队列获取当前的玩家输入.
    // 
    // InstantCommand 立即指令: 仅当前帧有效.
    // TimeoutCommand 延时指令: 经过一段时间之后会失效.
    // IntervalCommand 区间指令: 玩家完成某种操作(比如释放按下的按键)之后会失效.
    // 
    // 指令队列: 对当前存在的所有指令排序.
    // 

    public enum CommandType
    {
        Instant = 0,
        Timeout,
        Interval,
    }
    
    [Serializable]
    public struct CommandKey
    {
        public KeyCode key;
        public bool ctrl;
        public bool shift;
        public CommandKey(KeyCode key, bool ctrl, bool shift)
            => (this.key, this.ctrl, this.shift) = (key, ctrl, shift);
    }

    public static class CommandQueue
    {
        public sealed class Callbacks
        {
            public Action create = () => { };
            public Action delete = () => { };
        }

        /// <summary>
        /// 存储创建一个指令时需要的信息.
        /// </summary>
        abstract class CreateInfo
        {
            public int priority;
            public CommandKey key;
            public Callbacks callbacks;

            public abstract Command Create();

            public sealed class Instant : CreateInfo
            {
                public override Command Create() => new Command.Instant() {
                    key = key,
                    priority = priority,
                    callbacks = callbacks,
                };
            }

            public sealed class Timeout : CreateInfo
            {
                public float duration;

                public override Command Create() => new Command.Timeout() {
                    key = key,
                    priority = priority,
                    callbacks = callbacks,
                    endTime = Time.time + duration,
                };
            }

            public sealed class Interval : CreateInfo
            {
                public override Command Create() => new Command.Interval() {
                    key = key,
                    priority = priority,
                    callbacks = callbacks,
                };
            }
        }

        abstract class Command
        {
            public int priority;
            public Callbacks callbacks;
            public CommandKey key;

            public sealed class Instant : Command { }
            public sealed class Timeout : Command { public float endTime; }
            public sealed class Interval : Command { }
        }

        /// <summary>
        /// 指令生成器.
        /// </summary>
        readonly static Dictionary<CommandKey, CreateInfo> creators = new Dictionary<CommandKey, CreateInfo>();

        /// <summary>
        /// 按照顺序排序的指令.
        /// </summary>
        readonly static SortedList<int, HashSet<Command>> sortedCmds = new SortedList<int, HashSet<Command>>();

        /// <summary>
        /// 使用名称索引的指令.
        /// </summary>
        readonly static Dictionary<CommandKey, Command> keyCmds = new Dictionary<CommandKey, Command>();

        /// <summary>
        /// 把一个玩家输入绑定到一个命令上.
        /// </summary>
        public static void BindInstant(CommandKey key, int priority, Callbacks callbacks = null)
        {
            creators.Add(key, new CreateInfo.Instant() {
                priority = priority,
                key = key,
                callbacks = callbacks,
            });
        }

        /// <summary>
        /// 把一个玩家输入绑定到一个命令上.
        /// </summary>
        public static void BindTimeout(CommandKey key, float duration, int priority, Callbacks callbacks = null)
        {
            creators.Add(key, new CreateInfo.Timeout() {
                priority = priority,
                key = key,
                callbacks = callbacks,
                duration = duration,
            });;
        }

        /// <summary>
        /// 把一个玩家输入绑定到一个命令上.
        /// </summary>
        public static void BindInterval(CommandKey key, int priority, Callbacks callbacks = null)
        {
            creators.Add(key, new CreateInfo.Interval() {
                priority = priority,
                key = key,
                callbacks = callbacks,
            });
        }


        /// <summary>
        /// 解除玩家输入对应的绑定.
        /// </summary>
        public static void Unbind(CommandKey key) => creators.Remove(key);

        /// <summary>
        /// 创建各种事件.
        /// </summary>
        public static void Create()
        {
            foreach(KeyCode kb in Enum.GetValues(typeof(KeyCode)))
            {
                if(!Input.GetKeyDown(kb)) continue;
                if(kb == KeyCode.LeftControl || kb == KeyCode.RightControl) continue;
                if(kb == KeyCode.LeftShift || kb == KeyCode.RightShift) continue;
                bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                bool TryCreateCommand(bool c, bool s)
                {
                    var key = new CommandKey(kb, c, s);
                    if(!creators.TryGetValue(key, out var info)) return false;
                    var resCmd = info.Create();
                    switch(resCmd)
                    {
                    case Command.Timeout x:
                        // 如果这个指令原来就有, 则用新的指令替换它而不触发回调.
                        var prevCmd = keyCmds.GetOrDefault(info.key, null);
                        if(prevCmd != null)
                        {
                            sortedCmds[info.priority].Remove(prevCmd);
                            keyCmds[info.key] = null;
                        }
                        else // 否则新建指令, 并触发回调.
                        {
                            info.callbacks?.create();
                        }
                        break;

                    default:
                        info.callbacks?.create();
                        break;
                    }

                    // 默认的添加操作.
                    keyCmds[info.key] = resCmd;
                    sortedCmds.GetOrDefault(info.priority).Add(resCmd);

                    return true;
                }
                if(ctrl && shift && TryCreateCommand(true, true)) continue;
                if(ctrl && TryCreateCommand(true, false)) continue;
                if(shift && TryCreateCommand(false, true)) continue;
                if(TryCreateCommand(false, false)) continue;
            }
        }

        /// <summary>
        /// 更新当前命令队列的各种命令(如果有的话).
        /// </summary>
        public static void Run()
        {
            // 目前不需要做什么事情...
        }

        /// <summary>
        /// 清理各种超时的, 失效的事件.
        /// </summary>
        public static void Clear()
        {
            int maxCount = 0;
            foreach(var i in sortedCmds) maxCount.UpdMax(i.Value.Count);
            var buffer = new Command[maxCount];
            foreach(var pix in sortedCmds)
            {
                var lst = pix.Value;

                void RemoveFromStorage(Command cmd)
                {
                    lst.Remove(cmd);
                    sortedCmds[cmd.priority].Remove(cmd);
                    keyCmds[cmd.key] = null;
                }

                // 把整个 list 复制下来, 方便删除.
                lst.CopyTo(buffer);
                foreach(var cmd in buffer.ToEnumerable(lst.Count))
                {
                    switch(cmd)
                    {
                    case Command.Instant xcmd:
                        // Instant 指令应当立即删除.
                        RemoveFromStorage(cmd);
                        cmd.callbacks?.delete();
                        break;

                    case Command.Timeout xcmd:
                        // Timeout 指令在超时之后删除.
                        if(xcmd.endTime < Time.time)
                        {
                            RemoveFromStorage(cmd);
                            cmd.callbacks?.delete();
                        }
                        break;

                    case Command.Interval xcmd:
                        // Interval 指令在接收到对应的 key 之后删除.
                        if(Input.GetKeyUp(xcmd.key.key))
                        {
                            RemoveFromStorage(cmd);
                            cmd.callbacks?.delete();
                        }
                        break;

                    default:
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 判断当前队列中是否有该命令.
        /// </summary>
        public static bool Get(CommandKey key) => keyCmds.GetOrDefault(key, null) != null;


        /// <summary>
        /// 判断该命令是否处于队头位置.
        /// </summary>
        public static bool Top(CommandKey key) 
            => keyCmds.GetOrDefault(key, null) != null 
            && sortedCmds.Count != 0 && sortedCmds.First().Value.Contains(keyCmds[key]);
    }


}