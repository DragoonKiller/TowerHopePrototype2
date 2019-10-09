using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Systems;
    using Utils;

    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraFollowMouse : MonoBehaviour
    {
        public bool active;
        
        [Tooltip("跟踪者.")]
        public Transform target;
        
        [Tooltip("跟踪对象.")]
        public Transform following;
        
        [Tooltip("相机到跟踪对象的垂直距离.")]
        public float height;
        
        [Tooltip("世界坐标系下, 相机开始偏移需要鼠标距离跟踪对象多远.")]
        public float minDist;
        
        [Tooltip("世界坐标系下, 鼠标与跟踪对象的距离超过该值就不再往远处偏移.")]
        public float maxDist;
        
        [Tooltip("最大偏移距离.")]
        public float offsetDist;
        
        [Tooltip("距离偏移的系数曲线.")]
        public AnimationCurve distCurve;
        
        Vector2 fpos => following.position;
        
        CameraFollowMouse()
        {
            Signal<Signals.PostUpdate>.Listen(Step);
        }

        void OnDestroy()
        {
            Signal<Signals.PostUpdate>.Remove(Step);
        }

        void Step(Signals.PostUpdate e)
        {
            if(!active) return;
            
            var delta = ExCursor.worldPos - fpos;
            var dir = delta.normalized;
            var dist = delta.magnitude;
            
            dist = dist.Xmap(minDist, maxDist, 0, 1).Clamp(0, 1);
            dist = distCurve.Evaluate(dist).Clamp(0, 1) * offsetDist;
            
            var wpos = fpos + dist * dir;
            Debug.DrawLine(fpos, fpos + dist * dir, Color.yellow);
            target.position = wpos.ToVec3().Z(height);
        }
    }

}
