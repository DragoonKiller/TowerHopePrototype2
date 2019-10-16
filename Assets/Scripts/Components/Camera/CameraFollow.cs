using System;
using UnityEngine;
using Utils;

namespace Tower.Components
{
    using Systems;

    /// <summary>
    /// 控制 target 跟踪 follow 目标, 并以 offset 作偏移.
    /// 在 offset 相对 follow 偏移为 0 时, target 会直接快速跟随 following.
    /// 如果 offset 有偏移, 摄像机会以一个曲线跟踪该偏移位置.
    /// <summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("是否开启.")]
        public bool active;
        
        [Tooltip("摄像机跟踪的主要物体.")]
        public Transform target;
        
        [Tooltip("摄像机跟踪的偏移物体.")]
        public Transform offsetTarget;
        
        [Tooltip("对 target 的每帧逼近率.")]
        [Range(0, 1)] public float closeRate;
        
        [Tooltip("对 offsetTarget 的每帧逼近率.")]
        [Range(0, 1)] public float offsetCloseRate;
        
        [Tooltip("摄像机目前的位置相对 target 的位置偏移是多少.")]
        public Vector2 offset;
        
        [Tooltip("摄像机在到达指定目标点的基础上再偏移多少.")]
        public Vector3 fixedOffset;
        
        [Tooltip("摄像机的最小逼近速度.")]
        public float minSpeed;
        
        [Tooltip("摄像机的跟踪限制盒占整个视野的比例.")]
        [Range(0, 1f)] public float limitRatio;

        Camera cam => this.GetComponent<Camera>();

        [Header("Info")]
        [SerializeField] public Vector2 localRelativePosition;

        CameraFollow()
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
            UpdatePos(e.dt);
            LimitArea();   
        }
        
        void UpdatePos(float dt)
        {
            // 当前不考虑 offset 的摄像机所在位置.
            var focusPos = (this.transform.position - fixedOffset - offset.ToVec3()).ToVec2();    
            // 这个摄像机需要移动到的目标位置.
            var targetFocusPos = target.position.ToVec2();
            // 计算不带 offset 情况下的位移向量.
            var nonOffsetMove = focusPos.To(targetFocusPos);
            var moveDist = nonOffsetMove.magnitude * MoveMult(closeRate, dt);
            moveDist = moveDist.Max(minSpeed).Min(nonOffsetMove.magnitude);
            nonOffsetMove = nonOffsetMove.normalized * moveDist;
            var nextFocusPos = nonOffsetMove + focusPos;
            
            // 当前摄像机的 offset.
            var curOffset = offset;
            // 当前目标的 offset.
            var targetOffset = (offsetTarget.position - target.position).ToVec2();
            // 计算针对 offset 的位移向量.
            var offsetMove = curOffset.To(targetOffset);
            var offsetDist = offsetMove.magnitude * MoveMult(offsetCloseRate, dt);
            offsetDist = offsetDist.Max(minSpeed).Min(offsetMove.magnitude);
            offsetMove = offsetMove.normalized * offsetDist;
            var nextOffset = offsetMove + curOffset;
            offset = nextOffset;
            
            // 处理最终位置.
            this.transform.position = (nextFocusPos + nextOffset).ToVec3() + fixedOffset;
        }
        
        void LimitArea()
        {
            var size = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);
            var curPos = this.transform.position - fixedOffset;
            var lb = this.transform.position.ToVec2() - size * 0.5f;
            var rt = this.transform.position.ToVec2() + size * 0.5f;
            curPos.x = curPos.x.Clamp(lb.x, rt.x);
            curPos.y = curPos.y.Clamp(lb.y, rt.y);
            Debug.DrawLine(lb, new Vector2(lb.x, rt.y), Color.red);
            Debug.DrawLine(lb, new Vector2(rt.x, lb.y), Color.red);
            Debug.DrawLine(rt, new Vector2(lb.x, rt.y), Color.red);
            Debug.DrawLine(rt, new Vector2(rt.x, lb.y), Color.red);
            this.transform.position = curPos + fixedOffset;
        }
        
        
        float MoveMult(float ratePerSec, float t) => 1 - ratePerSec.Pow(t);
    }
}
