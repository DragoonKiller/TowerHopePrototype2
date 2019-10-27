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
        
        /// <summary>
        /// 主摄像机渲染流程. 除了绘制一般的游戏内容以外, 还要进行后处理.
        /// </summary>
        public override void Render(ScriptableRenderContext context, RenderSystem renderer)
        {
            Debug.Assert(cam.targetTexture == null);
            
            // 扩展摄像机渲染范围.
            cam.orthographicSize *= data.mainCamereSizeMult;
            
            // 绑定摄像机参数.
            context.SetupCameraProperties(cam);
            
            // 设置临时渲染目标.
            var size = new Vector2Int(cam.pixelWidth, cam.pixelHeight);
            size.x = (size.x * data.mainCameraResolutionMult * data.mainCamereSizeMult).FloorToInt();
            size.y = (size.y * data.mainCameraResolutionMult * data.mainCamereSizeMult).FloorToInt();
            new RenderingUtils.RenderTextureBuffer("Main", context, size).WithTextures((a, b) =>
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
                    data.subSampleMaterial.SetVector("_SubSamplePivot", new Vector2(0.5f, 0.5f));
                    data.subSampleMaterial.SetFloat("_SubSampleRate", data.mainCameraSampleMult);
                    x.Blit(a, BuiltinRenderTextureType.CameraTarget, data.subSampleMaterial);
                });
            });
            
            // 恢复摄像机渲染范围.
            cam.orthographicSize /= data.mainCamereSizeMult;
        }
    }

}
