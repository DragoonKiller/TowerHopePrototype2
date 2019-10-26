using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tower.UI
{
    using Systems;
    
    /// <summary>
    /// 渲染流程 RenderSystem 中会用到这个组件的成员.
    /// </summary>
    [ExecuteAlways]
    public sealed class MinimapCamera : Systems.CustomRendering
    {
        public RenderTexture outTexture;
        public Material postEffect;
        public RenderTexture inTexture => this.GetComponent<Camera>().targetTexture;
        public override void PostRender(ScriptableRenderContext context)
        {
            context.ConsumeCommands(x => x.Blit(inTexture, outTexture, postEffect));
        } 
    }
}
