using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    
    /// <summary>
    /// 处理局部爆炸模糊的对象.
    /// </summary>
    public class LocalExplodePostEffect : MonoBehaviour
    {
        [Tooltip("绘制特效所需的材质.")]
        public Material mat;
        
        [Tooltip("最大影响范围.")]
        public float maxRadius;
        
        [Tooltip("扭曲强度.")]
        public float maxDeform;
        
        void Update()
        {
            RenderSystemAsset.inst.postRenderQueue.Enqueue(Setup);            
        }
        
        Material Setup()
        {
            mat.SetVector("_Center", this.transform.position);
            mat.SetFloat("_MaxRadius", maxRadius);
            mat.SetFloat("_MaxDeform", maxDeform);
            return mat;
        }
    }
    
    
}
