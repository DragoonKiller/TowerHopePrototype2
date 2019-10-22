﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tower.Rendering
{

    [CreateAssetMenu(menuName = "Render Pipeline/Create pipeline asset", fileName = "RenderSystem", order = 12999)]
    public class RenderSystemAsset : RenderPipelineAsset
    {
        public static RenderSystemAsset inst;
        public Color clearColor = Color.green;
        
        [Header("Main Camera")]
        public Material localExplodeEffect;
        
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