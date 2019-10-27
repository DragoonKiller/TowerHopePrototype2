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
