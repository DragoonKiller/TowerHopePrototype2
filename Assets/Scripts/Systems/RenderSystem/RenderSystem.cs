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
            var x = cam.GetComponent<CustomRendering>();
            if(x) x.PreRender(context);
            context.SetupCameraProperties(cam);
            OrdinaryDraw(cam, context);
            if(x) x.PostRender(context);
        }
        
        /// <summary>
        /// 主摄像机渲染流程. 除了绘制一般的游戏内容以外, 还要进行后处理.
        /// </summary>
        void MainCameraRendering(Camera cam, ScriptableRenderContext context)
        {
            Debug.Assert(cam.targetTexture == null);
            
            // 扩展摄像机渲染范围.
            cam.orthographicSize *= data.mainCamereSizeMult;
            
            // 绑定摄像机参数.
            context.SetupCameraProperties(cam);
            
            // 设置临时渲染目标.
            var resolution = new Vector2Int(cam.pixelWidth, cam.pixelHeight);
            resolution.x = (resolution.x * data.mainCameraResolutionMult * data.mainCamereSizeMult).FloorToInt();
            resolution.y = (resolution.y * data.mainCameraResolutionMult * data.mainCamereSizeMult).FloorToInt();
            new RenderingUtils.RenderTextureBuffer(resolution, context).WithTextures((a, b) =>
            {
                // 改变绘制目标.
                context.ConsumeCommands(x => x.SetRenderTarget(a));    
            
                // 常规绘制部分.
                OrdinaryDraw(cam, context);
                
                // 应用特效.
                foreach(var f in data.postRenderQueue)
                {
                    context.ConsumeCommands(x => 
                    {
                        var mat = f();
                        
                        // 清空输出.
                        x.SetRenderTarget(b);
                        
                        // 只有颜色信息, 没有深度信息. 不需要清空深度.
                        x.ClearRenderTarget(false, true, new Color(0, 0, 0, 1));
                        
                        // 完成绘制.
                        x.Blit(a, b, mat);
                        
                        // 交换缓冲. 保证总是读取 a, 绘制 b.
                        (a, b) = (b, a);
                    });
                }
                
                data.postRenderQueue.Clear();
                
                // 把最终的图片绘制到摄像机的绘制目标(即屏幕)上.
                context.ConsumeCommands(x => 
                {
                    data.subSampleMaterial.SetVector("_SubSamplePivot", new Vector2(0.5f, 0.5f));
                    data.subSampleMaterial.SetFloat("_SubSampleRate", data.mainCameraSampleMult);
                    x.Blit(a, BuiltinRenderTextureType.CameraTarget, data.subSampleMaterial);
                });
            
            });
            
            // 恢复摄像机渲染范围.
            cam.orthographicSize /= data.mainCamereSizeMult;
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
            
            context.Submit();
        }
    }
}
