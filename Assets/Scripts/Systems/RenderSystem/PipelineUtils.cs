using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace Systems.Rendering
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
            // 提交所有绑定的渲染数据.
            context.Submit();
        }
        
        
        /// <summary>
        /// 双缓冲机制, 保存后处理的渲染目标.
        /// </summary>
        public struct RenderTextureBuffer
        {
            ScriptableRenderContext context;
            Vector2Int size;
            
            /// <summary>
            /// 双缓冲形式的后处理渲染目标.
            /// </summary>
            static RenderTexture[] sceneRenderTexture = new RenderTexture[2];
            
            /// <summary>
            /// 当前正在使用哪个渲染目标.
            /// </summary>
            static int curTexture;
            
            /// <summary>
            /// 获取当前正在使用的缓冲.
            /// </summary>
            static RenderTexture Sync(int id, Vector2Int size)
            {
                var cur = sceneRenderTexture[id];
                if(cur == null || cur.width != size.x || cur.height != size.y)
                {
                    var desc = new RenderTextureDescriptor(size.x, size.y);
                    desc.depthBufferBits = 0;
                    desc.colorFormat = RenderTextureFormat.ARGB32;
                    desc.enableRandomWrite = true;
                    desc.sRGB = true;
                    sceneRenderTexture[id] = cur = new RenderTexture(desc);
                    sceneRenderTexture[id].name = $"Temp Render Target {id}";
                }
                return cur;
            }
            
            /// <summary>
            /// 交换缓冲.
            /// </summary>
            static void Swap() => curTexture ^= 1;
            
            /// <summary>
            /// 构造函数. 
            /// </summary>
            public RenderTextureBuffer(Vector2Int size, ScriptableRenderContext context) => (this.size, this.context) = (size, context);
            
            /// <summary>
            /// 直接提供这两个缓冲.
            /// </summary>
            public RenderTextureBuffer WithTextures(Action<RenderTexture, RenderTexture> f)
            {
                var x = Sync(curTexture, size);
                var y = Sync(curTexture ^ 1, size);
                f(x, y);
                return this;
            }
        }

    }
    
}
