using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tower.Components
{
    using Utils;
    using Systems;
    
    public class BackgroundCameraRenderer : CustomRendering
    {
        [Tooltip("绘制背景模糊的材质.")]
        public Material backBlurMaterial;
        
        [Tooltip("用于模糊的画布, 其大小是摄像机画布大小乘以该常数.")]
        [Range(0.1f, 1)] public float rescaleMult;
        
        [Tooltip("进行多少次模糊渲染.")]
        [Range(1, 5)] public int blurCount; 
        
        public override void Render(ScriptableRenderContext context, RenderSystem renderer)
        {
            // 绘制背景颜色.
            context.SetupCameraProperties(cam);
            context.ConsumeCommands(x => x.ClearRenderTarget(true, true, cam.backgroundColor));
            if((cam.clearFlags & CameraClearFlags.Skybox) != 0) context.DrawSkybox(cam);
            
            // 保存原始的 CullingMask. 之后要修改摄像机的 CullingMask.
            var originCameraCullingMask = cam.cullingMask;
            
            // 按照 mask 的先后顺序绘制背景, 并做模糊处理以达到景深效果.
            // 假设绘制层按照layermask编号从小到大排序, 再次编号: 1, 2, ..., n.
            // 其中 t 为中间层. 绘制顺序如下:
            // [n] [blur] [n-1] [blur] ... [t+1] [blur] [t]
            // 混合模式是正常的叠加.
            var size = new Vector2Int(
                (cam.pixelWidth * rescaleMult).FloorToInt(), 
                (cam.pixelHeight * rescaleMult).FloorToInt()
            );
            
            new RenderingUtils.RenderTextureBuffer("Background", context, size, FilterMode.Trilinear).WithTextures((a, b) => 
            {
                void Draw(RenderTexture dest)
                {
                    if(!cam.TryGetCullingParameters(out var culls)) return;
                    var cullRes = context.Cull(ref culls);
                    var sortSettings = new SortingSettings(cam) { criteria = SortingCriteria.CommonTransparent };
                    var drawSettings = new DrawingSettings(RenderSystem.unlitShaderTagId, sortSettings);
                    var filterSettings = new FilteringSettings(RenderQueueRange.all);
                    context.ConsumeCommands(x => x.SetRenderTarget(dest));
                    context.DrawRenderers(cullRes, ref drawSettings, ref filterSettings);
                    context.Submit();
                }
                
                // 清理颜色缓冲.
                context.ConsumeCommands(x => 
                {
                    x.SetRenderTarget(a);
                    x.ClearRenderTarget(true, true, cam.backgroundColor);
                    x.SetRenderTarget(b);
                    x.ClearRenderTarget(true, true, cam.backgroundColor);
                });
                
                // 背景模糊流程.
                for(int i = 0; i < 31; i++)
                {
                    var render = ((1 << i) & originCameraCullingMask) != 0;
                    if(!render) continue;
                    
                    cam.cullingMask = 1 << i;
                    context.SetupCameraProperties(cam);
                    
                    // 常规绘制场景.
                    Draw(b);
                    (a, b) = (b, a);
                    
                    // 模糊.
                    context.ConsumeCommands("Background: Blur", x => 
                    {
                        for(int j = 0; j < blurCount; j++)
                        {
                            x.Blit(a, b, backBlurMaterial);
                            (a, b) = (b, a);
                        }
                    });
                    
                    // 下次画场景直接在当前的输出上画.
                    (a, b) = (b, a);
                }
                
                // 画到摄像机的渲染目标上.
                cam.cullingMask = originCameraCullingMask;
                context.SetupCameraProperties(cam);
                context.ConsumeCommands("Background: Final Blit", x => x.Blit(b, BuiltinRenderTextureType.CameraTarget));
            });
        }
    }
}
