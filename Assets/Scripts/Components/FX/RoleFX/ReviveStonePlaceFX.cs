using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Utils;
    
    /// <summary>
    /// 角色放置复活石的动画控制脚本.
    /// </summary>
    [ExecuteAlways]
    public class ReviveStonePlaceFX : MonoBehaviour
    {
        public float process;
        
        public Vector2 baseScale;
        public AnimationCurve scaleCurve;
        
        public Color baseColor;
        public AnimationCurve alphaCurve;
        
        SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
        
        void Update()
        {
            // 在编辑器中, 按照 baseScale 和 baseAlpha 更新 SpriteRenderer.
            if(Application.isEditor && !Application.isPlaying)
            {
                rd.color = baseColor;
                this.transform.localScale = baseScale;
                return;
            }
            
            if(process == 0)
            {
                rd.enabled = false;
            }
            else
            {
                rd.enabled = true;
                this.transform.localScale = baseScale * scaleCurve.Evaluate(process);
                rd.color = baseColor.A(baseColor.a * alphaCurve.Evaluate(process));
            }
        }
    }

}
