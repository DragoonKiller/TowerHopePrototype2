using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace Tower.Rendering
{
    public static class Utils
    {
        [ThreadStatic]
        static CommandBuffer cmds = new CommandBuffer() { name = "shared command buffer" };
            
        public static void ConsumeCommands(this ScriptableRenderContext context, CommandBuffer buffer)
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }
        
        public static void ConsumeCommands(this ScriptableRenderContext context, Action<CommandBuffer> f)
        {
            // 渲染线程独立.
            f(cmds);
            context.ExecuteCommandBuffer(cmds);
            cmds.Clear();
        }
    }
    
}
