using UnityEngine;
using UnityEngine.Rendering;
using System;

namespace Systems
{
    /// <summary>
    /// 自定义渲染流程. 这个东西用于将单独摄像机的渲染流程和 RenderSystem 解耦.
    /// 继承这个类, 并且把这个脚本挂在摄像机上, 就可以使用它.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public abstract class CustomRendering : MonoBehaviour
    {
        protected RenderSystemAsset data => RenderSystemAsset.inst;
        protected Camera cam => this.GetComponent<Camera>();
                
        public abstract void Render(ScriptableRenderContext context, RenderSystem renderer);
    }
}
