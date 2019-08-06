using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tower.Components
{
    using Systems;

    [RequireComponent(typeof(LineRenderer))]
    public class ObjectAnimMagicFlare : MonoBehaviour
    {
        [Tooltip("火花的初始方向.")]
        public Vector2 dir;

        [Tooltip("(角度)火花的最大偏角.起始偏角即此数值.")]
        public float angle;

        [Tooltip("速度的大小和方向回到初始值的时间.")]
        public float peroid;

        [Tooltip("火花何时消失.")]
        public float duration;

        [Tooltip("Line renderer 的每个点, 是间隔多长时间采样得到的.")]
        public float sampleTime;

        [Tooltip("这个对象何时消失. 注意火花消失时, 由于残留一些轨迹, 所以该对象不能消失.")]
        public float lifeTime;

        LineRenderer lrd => GetComponent<LineRenderer>();

        float beginTime;

        void Start()
        {
            beginTime = Time.time;
            dir = dir.normalized;
            Signal<Signals.PostUpdate>.Listen(Step);
        }

        void OnDestroy()
        {
            Signal<Signals.PostUpdate>.Remove(Step);
        }

        void Step(Signals.PostUpdate e)
        {
            float curTime = Time.time;
            float usedTime = curTime - beginTime;
            if(usedTime >= lifeTime) { DestroyImmediate(this); return; }
            if(usedTime >= duration) return;
        }




    }

}
