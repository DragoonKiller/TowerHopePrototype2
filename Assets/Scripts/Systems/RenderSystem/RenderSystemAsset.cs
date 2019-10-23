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
        
        [Tooltip("主摄像机将会渲染预设值乘以这个常数的大小的屏幕, 以方面进行后处理以及超采样.")]
        [Range(1, 4)] public float mainCamereSizeMult;
        
        [Tooltip("最终视野大小和原屏幕相比的大小. 最后一步渲染处理会直接取图上的一部分, 这部分的大小由改参数决定.")]
        [Range(1, 2)] public float mainCameraSampleMult;
        
        /// <summary>
        /// 后处理操作队列. 每帧会依次取队列中的函数对象执行并销毁.
        /// 队列中的函数负责设置好后处理的材质的各项参数后返回该材质, 以执行 CommandBuffer.Blit.
        /// </summary>
        public Queue<Func<Material>> postRenderQueue = new Queue<Func<Material>>();
        
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
