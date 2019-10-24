using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    
    /// <summary>
    /// 处理局部爆炸模糊的对象.
    /// </summary>
    [ExecuteAlways]
    public class LocalExplodePostEffect : MonoBehaviour
    {
        [Tooltip("绘制特效所需的材质.")]
        public Material mat;
        
        [Tooltip("该对象的留存时间.")]
        public float lifeTime;
        
        [Tooltip("最大影响范围.")]
        public float maxRadius;
        
        [Tooltip("扭曲强度.")]
        public float maxDeform;
        
        [Header("Debug")]
        
        [Tooltip("该对象的创建时间.")]
        [SerializeField] float beginTime;
        
        void Start()
        {
            beginTime = Time.time;
        }
        
        void Update()
        {
            if(Time.time - beginTime > lifeTime)
            {
                DestroyImmediate(this.gameObject);
                return;
            }
            
            RenderSystemAsset.inst.postRenderQueue.Add(Setup);            
        }
        
        Material Setup()
        {
            mat.SetVector("_Center", this.transform.position);
            mat.SetVector("_CameraViewport", new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize));
            mat.SetFloat("_MaxRadius", maxRadius);
            mat.SetFloat("_MaxDeform", maxDeform);
            return mat;
        }
    }
    
    
}
