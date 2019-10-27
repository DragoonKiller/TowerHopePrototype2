using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Rendering.RenderPipeline;

namespace Systems
{
    using Utils;
    
    /// <summary>
    /// 定义一整套渲染流程.
    /// </summary>
    public sealed class RenderSystem : RenderPipeline
    {
        public RenderSystemAsset data => RenderSystemAsset.inst;
        
        public static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        
        public static ShaderTagId[] legacyShaderTagIds = {
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
                if(cam.name == "SceneCamera") 
                {
                    SceneCameraRendering(cam, context);
                    continue;
                }
                
                if(!cam.gameObject.activeSelf) continue;
                if(!cam.enabled) continue;
                
                if(cam.TryGetComponent<CustomRendering>(out var customRendering)) customRendering.Render(context, this);
                else InGameCameraRendering(cam, context);
            }
        }
        
        /// <summary>
        /// 场景摄像机的渲染流程.
        /// </summary>
        void SceneCameraRendering(Camera cam, ScriptableRenderContext context)
        {
            context.SetupCameraProperties(cam);
            if(Handles.ShouldRenderGizmos()) context.DrawGizmos(cam, GizmoSubset.PreImageEffects);
            InGameCameraRendering(cam, context);
            if(Handles.ShouldRenderGizmos()) context.DrawGizmos(cam, GizmoSubset.PostImageEffects);
            context.Submit();
            
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
        /// 根据摄像机设置清理其目标缓冲.
        /// </summary>
        public void Clear(Camera cam, ScriptableRenderContext context)
        {
            context.ConsumeCommands(x => x.ClearRenderTarget(true, true, cam.backgroundColor));
            if((cam.clearFlags & CameraClearFlags.Skybox) != 0) context.DrawSkybox(cam);
            
        }
        
        /// <summary>
        /// 绘制游戏内物体的流程. 不包含各种预处理和后处理.
        /// </summary>
        public void OrdinaryDraw(Camera cam, ScriptableRenderContext context)
        {
            if(!cam.TryGetCullingParameters(out var culls)) return;
            var cullRes = context.Cull(ref culls);
            
            var sortSettings = new SortingSettings(cam) { criteria = SortingCriteria.CommonOpaque };
            var drawSettings = new DrawingSettings(unlitShaderTagId, sortSettings);
            var filterSettings = new FilteringSettings(RenderQueueRange.all);
            context.DrawRenderers(cullRes, ref drawSettings, ref filterSettings);
            
            context.Submit();
        }
    }
}
