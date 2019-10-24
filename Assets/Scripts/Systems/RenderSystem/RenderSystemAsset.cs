using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Systems
{

    [CreateAssetMenu(menuName = "Render Pipeline/Create pipeline asset", fileName = "RenderSystem", order = 12999)]
    public class RenderSystemAsset : RenderPipelineAsset
    {
        public static RenderSystemAsset inst;
        
        [Tooltip("清屏颜色.")]
        public Color clearColor = Color.green;
        
        [Tooltip("超采样和子视口变换材质.")]
        public Material subSampleMaterial;
        
        
        [Tooltip("初始渲染分辨率乘数.")]
        [Range(1, 4)] public float mainCameraResolutionMult;
        
        [Tooltip("主摄像机渲染的场景大小乘数.")]
        [Range(1, 4)] public float mainCamereSizeMult;
        
        [Tooltip("取主摄像机渲染图片的多少来画在最终的屏幕上.")]
        [Range(1e-4f, 1)] public float mainCameraSampleMult;
        
        /// <summary>
        /// 后处理操作队列. 每帧会依次取队列中的函数对象执行并销毁.
        /// 队列中的函数负责设置好后处理的材质的各项参数后返回该材质, 以执行 CommandBuffer.Blit.
        /// </summary>
        public List<Func<Material>> postRenderQueue = new List<Func<Material>>();
        
        RenderSystemAsset()
        {
            inst = this;
        }

        protected override RenderPipeline CreatePipeline()
        {
            return new RenderSystem();
        }
    }

}
