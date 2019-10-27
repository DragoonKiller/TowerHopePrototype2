using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tower.Components
{
    using Utils;
    using Systems;
    
    public class MainCameraRenderer : CustomRendering
    {
        public RenderTexture background;
        
        
        [Tooltip("超采样和子视口变换材质.")]
        public Material subSampleMaterial;
        
        [Tooltip("初始渲染分辨率乘数.")]
        [Range(1, 4)] public float resolutionMult;
        
        [Tooltip("主摄像机渲染的场景大小乘数.")]
        [Range(1, 4)] public float sizeMult;
        
        [Tooltip("取主摄像机渲染图片的多少来画在最终的屏幕上.")]
        [Range(1e-4f, 1)] public float sampleMult;
        
        
        /// <summary>
        /// 主摄像机渲染流程. 除了绘制一般的游戏内容以外, 还要进行后处理.
        /// </summary>
        public override void Render(ScriptableRenderContext context, RenderSystem renderer)
        {
            Debug.Assert(cam.targetTexture == null);
            
            // 扩展摄像机渲染范围.
            cam.orthographicSize *= sizeMult;
            
            // 绑定摄像机参数.
            context.SetupCameraProperties(cam);
            
            // 设置临时渲染目标.
            var size = new Vector2Int(cam.pixelWidth, cam.pixelHeight);
            size.x = (size.x * resolutionMult * sizeMult).FloorToInt();
            size.y = (size.y * resolutionMult * sizeMult).FloorToInt();
            new RenderingUtils.RenderTextureBuffer("Main", context, size, FilterMode.Bilinear).WithTextures((a, b) =>
            {
                // 绘制预处理后的背景.
                context.ConsumeCommands(x => x.Blit(background, a));
                
                // 常规绘制部分.
                renderer.OrdinaryDraw(cam, context);
                
                // 应用特效.
                foreach(var f in data.postRenderQueue)
                {
                    context.ConsumeCommands(x => 
                    {
                        var mat = f();
                        
                        // 清空输出.
                        // 只有颜色信息, 没有深度信息. 不需要清空深度.
                        x.SetRenderTarget(b);
                        x.ClearRenderTarget(false, true, new Color(0, 0, 0, 1));
                        
                        // 完成绘制.
                        x.Blit(a, b, mat);
                        (a, b) = (b, a);
                    });
                }
                
                data.postRenderQueue.Clear();
                
                // 把最终的图片绘制到摄像机的绘制目标(即屏幕)上.
                context.ConsumeCommands(x => 
                {
                    subSampleMaterial.SetVector("_SubSamplePivot", new Vector2(0.5f, 0.5f));
                    subSampleMaterial.SetFloat("_SubSampleRate", sampleMult);
                    x.Blit(a, BuiltinRenderTextureType.CameraTarget, subSampleMaterial);
                });
            });
            
            // 恢复摄像机渲染范围.
            cam.orthographicSize /= sizeMult;
        }
    }

}
