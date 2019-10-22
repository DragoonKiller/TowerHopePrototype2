using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Rendering.RenderPipeline;

namespace Tower.Rendering
{

    public class RenderSystem : RenderPipeline
    {
        public RenderSystemAsset asset => RenderSystemAsset.inst;
        
        static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        
        static ShaderTagId[] legacyShaderTagIds = {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };
        
        public RenderSystem() 
        {
            
        }
        
        protected override void Render(ScriptableRenderContext context, Camera[] cams)
        {
            foreach(var cam in cams)
            {
                context.SetupCameraProperties(cam);
                
                context.ConsumeCommands(x => x.BeginSample(cam.name));
                
                context.ConsumeCommands(x => x.ClearRenderTarget(true, true, cam.backgroundColor));
                
                if((cam.clearFlags & CameraClearFlags.Skybox) != 0) context.DrawSkybox(cam);
                
                if(!cam.TryGetCullingParameters(out var culls)) continue;
                var cullRes = context.Cull(ref culls);
                
                var sortSettings = new SortingSettings(cam) { criteria = SortingCriteria.RenderQueue };
                var drawSettings = new DrawingSettings(unlitShaderTagId, sortSettings);
                var filterSettings = new FilteringSettings(RenderQueueRange.all);
                context.DrawRenderers(cullRes, ref drawSettings, ref filterSettings);
                
                context.ConsumeCommands(x => x.EndSample(cam.name));
                
                #if UNITY_EDITOR
                if(Handles.ShouldRenderGizmos()) context.DrawGizmos(cam, GizmoSubset.PreImageEffects);
                #endif
                
                #if UNITY_EDITOR
                if(Handles.ShouldRenderGizmos()) context.DrawGizmos(cam, GizmoSubset.PostImageEffects);
                #endif
                
                context.Submit();
            }
        }
    }

}
