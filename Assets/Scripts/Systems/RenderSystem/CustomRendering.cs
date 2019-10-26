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
    public abstract class CustomRendering : MonoBehaviour
    {
        public virtual void PreRender(ScriptableRenderContext context) { }
        public virtual void PostRender(ScriptableRenderContext context) { }
    }
}
