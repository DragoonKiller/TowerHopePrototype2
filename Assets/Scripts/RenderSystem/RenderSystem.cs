using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Rendering.RenderPipeline;

namespace Tower.Rendering
{
    /// <summary>
    /// 双缓冲机制, 保存后处理的渲染目标.
    /// </summary>
    struct RenderTextureBuffer
    {
        ScriptableRenderContext context;
        Camera camera;
        
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
        static RenderTexture Sync(int id, Camera cam)
        {
            var cur = sceneRenderTexture[id];
            if(cur == null || cur.width != cam.pixelWidth || cur.height != cam.pixelHeight)
            {
                sceneRenderTexture[id] = cur = new RenderTexture(
                    cam.pixelWidth,
                    cam.pixelHeight,
                    24,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Default
                );
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
        public RenderTextureBuffer(Camera cam, ScriptableRenderContext context) => (this.camera, this.context) = (cam, context);
        
        /// <summary>
        /// 直接提供这两个缓冲.
        /// </summary>
        public RenderTextureBuffer WithTextures(Action<RenderTexture, RenderTexture> f)
        {
            var x = Sync(curTexture, camera);
            var y = Sync(curTexture ^ 1, camera);
            f(x, y);
            return this;
        }
    }

    /// <summary>
    /// 定义一整套渲染流程.
    /// </summary>
    public sealed class RenderSystem : RenderPipeline
    {
        public RenderSystemAsset data => RenderSystemAsset.inst;
        
        
        static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        
        static ShaderTagId[] legacyShaderTagIds = {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };
        
        protected override void Render(ScriptableRenderContext context, Camera[] cams)
        {
            foreach(var cam in cams)
            {
                if(Camera.main == cam) MainCameraRendering(cam, context);
                else if(cam.name == "SceneCamera") SceneCameraRendering(cam, context);
                else InGameCameraRendering(cam, context);
                context.Submit();
            }
        }
        
        /// <summary>
        /// 场景摄像机的渲染流程.
        /// </summary>
        void SceneCameraRendering(Camera cam, ScriptableRenderContext context)
        {
            InGameCameraRendering(cam, context);
            
            #if UNITY_EDITOR
            if(Handles.ShouldRenderGizmos() && cam.name == "SceneCamera") context.DrawGizmos(cam, GizmoSubset.PreImageEffects);
            #endif
            
            #if UNITY_EDITOR
            if(Handles.ShouldRenderGizmos() && cam.name == "SceneCamera") context.DrawGizmos(cam, GizmoSubset.PostImageEffects);
            #endif
        }
        
        /// <summary>
        /// 游戏内物体摄像机的渲染流程. 可以指定一个渲染目标, 以替代摄像机的渲染目标.
        /// </summary>
        void InGameCameraRendering(Camera cam, ScriptableRenderContext context)
        {
            context.SetupCameraProperties(cam);
            OrdinaryDraw(cam, context);
        }
        
        /// <summary>
        /// 主摄像机渲染流程. 除了绘制一般的游戏内容以外, 还要进行后处理.
        /// </summary>
        void MainCameraRendering(Camera cam, ScriptableRenderContext context)
        {
            Debug.Assert(cam.targetTexture == null);
            
            // 绑定摄像机参数.
            context.SetupCameraProperties(cam);
            
            new RenderTextureBuffer(cam, context).WithTextures((a, b) =>
            {
                // 改变绘制目标.
                context.ConsumeCommands(x => x.SetRenderTarget(a));    
            
                // 常规绘制部分.
                OrdinaryDraw(cam, context);
                
                // 后处理.
                context.ConsumeCommands(x =>
                {
                    // 应用特效: 局部爆炸.
                    x.Blit(a, b, data.localExplodeEffect);
                    
                    // 把最终的图片绘制到摄像机的绘制目标(即屏幕)上.
                    x.Blit(b, BuiltinRenderTextureType.CameraTarget);
                });
            });
        }
        
        /// <summary>
        /// 绘制游戏内物体的流程.
        /// </summary>
        void OrdinaryDraw(Camera cam, ScriptableRenderContext context)
        {
            context.ConsumeCommands(x => x.ClearRenderTarget(true, true, cam.backgroundColor));
            
            if((cam.clearFlags & CameraClearFlags.Skybox) != 0) context.DrawSkybox(cam);
            
            if(!cam.TryGetCullingParameters(out var culls)) return;
            var cullRes = context.Cull(ref culls);
            
            var sortSettings = new SortingSettings(cam) { criteria = SortingCriteria.RenderQueue };
            var drawSettings = new DrawingSettings(unlitShaderTagId, sortSettings);
            var filterSettings = new FilteringSettings(RenderQueueRange.all);
            context.DrawRenderers(cullRes, ref drawSettings, ref filterSettings);
        }
    }
}
