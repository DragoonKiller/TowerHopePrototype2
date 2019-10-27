using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;

namespace Systems
{
    using Utils;
    
    public static class RenderingUtils
    {
        static string sharedCommandBufferName = "shared command buffer";
        
        [ThreadStatic]
        static CommandBuffer cmds = new CommandBuffer() { name = sharedCommandBufferName };
            
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
        
        public static void ConsumeCommands(this ScriptableRenderContext context, string name, Action<CommandBuffer> f)
        {
            cmds.name = name;
            context.ConsumeCommands(f);
            cmds.name = sharedCommandBufferName;
        }
        
        
        /// <summary>
        /// 双缓冲贴图, 保存后处理的渲染目标.
        /// 同一个渲染操作应当使用统一的标识; 不同的渲染操作应当使用不同的渲染标识.
        /// 这样可以避免因为渲染参数不一致而频繁创建和销毁贴图.
        /// </summary>
        public struct RenderTextureBuffer
        {
            ScriptableRenderContext context;
            string name;
            Vector2Int size;
            public FilterMode filter;
            public TextureWrapMode wrap;
            
            static Dictionary<string, RenderTexture[]> textures = new Dictionary<string, RenderTexture[]>();
            
            /// <summary>
            /// 当前正在使用哪个渲染目标.
            /// </summary>
            static int curTexture;
            
            /// <summary>
            /// 获取当前正在使用的缓冲.
            /// </summary>
            static RenderTexture Sync(string name, int id, Vector2Int size, FilterMode filterMode, TextureWrapMode wrapMode)
            {
                var tex = textures.GetOrDefault(name, new RenderTexture[2]);
                var cur = tex[id];
                if(cur == null || cur.width != size.x || cur.height != size.y)
                {
                    var desc = new RenderTextureDescriptor(size.x, size.y);
                    desc.depthBufferBits = 0;
                    desc.colorFormat = RenderTextureFormat.ARGB32;
                    desc.enableRandomWrite = true;
                    desc.sRGB = true;
                    if(tex[id] != null) tex[id].Release();
                    tex[id] = cur = new RenderTexture(desc);
                    tex[id].name = $"{name} Temp {id}";
                    tex[id].filterMode = filterMode;
                    tex[id].wrapMode = wrapMode;
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
            public RenderTextureBuffer(
                string name,
                ScriptableRenderContext context, 
                Vector2Int size, 
                FilterMode filter = FilterMode.Point,
                 TextureWrapMode wrap = TextureWrapMode.Clamp
            ) => (this.name, this.context, this.size, this.filter, this.wrap) = (name, context, size, filter, wrap);
            
            /// <summary>
            /// 直接提供这两个缓冲.
            /// </summary>
            public RenderTextureBuffer WithTextures(Action<RenderTexture, RenderTexture> f)
            {
                var x = Sync(name, curTexture, size, filter, wrap);
                var y = Sync(name, curTexture ^ 1, size, filter, wrap);
                f(x, y);
                return this;
            }
        }

    }
    
}
