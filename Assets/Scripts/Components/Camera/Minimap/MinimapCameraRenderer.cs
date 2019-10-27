using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tower.Components
{
    using Systems;
    
    /// <summary>
    /// 渲染流程 RenderSystem 中会用到这个组件的成员.
    /// </summary>
    [ExecuteAlways]
    public sealed class MinimapCameraRenderer : CustomRendering
    {
        public RenderTexture outTexture;
        public Material postEffect;
        public RenderTexture inTexture => this.GetComponent<Camera>().targetTexture;
        public override void Render(ScriptableRenderContext context, RenderSystem renderer)
        {
            context.SetupCameraProperties(cam);
            renderer.Clear(cam, context);
            renderer.OrdinaryDraw(cam, context);
            context.ConsumeCommands(x => x.Blit(inTexture, outTexture, postEffect));
        }
    }
}
