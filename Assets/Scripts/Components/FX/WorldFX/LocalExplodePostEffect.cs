using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;
    
    /// <summary>
    /// 处理局部爆炸模糊的对象.
    /// </summary>
    [ExecuteAlways]
    public class LocalExplodePostEffect : MonoBehaviour
    {
        [Tooltip("绘制特效所需的材质.")]
        public Material mat;
        
        [Tooltip("附加颜色.")]
        public Color additionalColor;
        
        [Tooltip("该对象的留存时间.")]
        public float lifeTime;
        
        [Tooltip("最小影响范围.")]
        public float minRadius;
        
        [Tooltip("最大影响范围.")]
        public float maxRadius;
        
        [Tooltip("半径变化规律.")]
        public AnimationCurve radiusCurve;
        
        [Tooltip("扭曲强度.")]
        public float maxDeform;
        
        [Tooltip("扭曲变化曲线.")]
        public AnimationCurve deformCurve;
        
        [Tooltip("扭曲的精细度.")]
        public float spaceFrequency;
        
        [Tooltip("扭曲随时间改变的频率.")]
        public float timeFrequency;
        
        [Tooltip("采样柏林噪声, 只有高于这个数值才会影响贴图.")]
        public float biasThreshold;
        
        [Header("Debug")]
        
        [Tooltip("该对象的创建时间.")]
        [SerializeField] float beginTime;
        
        void Start()
        {
            beginTime = Time.time;
            mat = new Material(mat);
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
            float process = (Time.time - beginTime) / lifeTime;
            
            mat.SetVector("_Center", this.transform.position);
            // OrthographicSize 是半宽/高. 乘以 2 变成真实的高度数值.
            mat.SetVector("_CameraViewport", 2 * new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize));
            mat.SetColor("_Color", additionalColor);
            mat.SetFloat("_TimeOffset", beginTime % 100.0f);
            mat.SetFloat("_MaxRadius", radiusCurve.Evaluate(process).Xmap(0, 1, minRadius, maxRadius));
            mat.SetFloat("_Deform", maxDeform * deformCurve.Evaluate(process));
            mat.SetFloat("_SpaceFrequency", spaceFrequency);
            mat.SetFloat("_TimeFrequency", timeFrequency);
            mat.SetFloat("_Process", process);
            mat.SetFloat("_BiasThreshold", biasThreshold);
            return mat;
        }
    }
    
    
}
