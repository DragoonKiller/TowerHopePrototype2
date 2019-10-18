using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    
    public static class DebugDraw
    {
        public static void Line(Vector2 from, Vector2 to, Color color) => Debug.DrawLine(from, to, color);
        public static void Line(Vector2 from, Vector2 to) => Debug.DrawLine(from, to);
        
        
        /// <summary>
        /// 注意: 只对外提供 2D 的 API.
        /// </summary>
        static void Circle(Vector3 center, Vector3 normal, float radius, Color color, int iteration = 24)
        {
            normal = normal.normalized;
            
            // 求一个在圆所处平面上的向量.
            var baseLine = Vector3.Cross(Vector3.up, normal);
            if(baseLine.magnitude <= Maths.eps) baseLine = Vector3.Cross(Vector3.forward, normal);
            baseLine = baseLine.normalized * radius;
            
            // 画圆.
            var unitAngle = Mathf.PI * 2 / iteration;
            for(int i=0; i<iteration; i++)
            {
                var curDir = Quaternion.AngleAxis(i * unitAngle * Mathf.Rad2Deg, normal) * baseLine;
                var nextDir = Quaternion.AngleAxis((i + 1) * unitAngle * Mathf.Rad2Deg, normal) * baseLine;
                Debug.DrawLine(center + curDir, center + nextDir, color);
            }
        }
        
        /// <summary>
        /// 画圆.
        /// </summary>
        public static void Circle(Vector2 center, float radius, Color color, int iteration = 24)
            => Circle(center, Vector3.forward, radius, color, iteration);
        
        /// <summary>
        /// 画圆.
        /// </summary>
        public static void Circle(Vector2 center, float radius, int iteration = 24)
            => Circle(center, Vector3.forward, radius, Color.white, iteration);
        
        /// <summary>
        /// 画方块. a, b, c, d 为顺时针或逆时针连续排列的方块顶点.
        /// </summary>
        public static void Rect(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
        {
            Debug.DrawLine(a, b, color);
            Debug.DrawLine(b, c, color);
            Debug.DrawLine(c, d, color);
            Debug.DrawLine(d, a, color);
        }
        
        /// <summary>
        /// 画方块. a, b, c, d 为顺时针或逆时针连续排列的方块顶点.
        /// </summary>
        public static void Rect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
            => Rect(a, b, c, d, Color.white);
        
        /// <summary>
        /// 画方块.
        /// </summary>
        public static void Rect(Rect rect, Color color)
            => Rect(rect.min, new Vector2(rect.min.x, rect.max.y), rect.max, new Vector2(rect.max.x, rect.min.y), color);
            
        /// <summary>
        /// 画方块.
        /// </summary>
        public static void DrawRect(Rect rect) => Rect(rect, Color.white);
        
            
        /// <summary>
        /// 画方块.
        /// </summary>
        public static void Rect(Vector2 offset, Rect rect, Color color)
            => Rect(
                offset + rect.min, 
                offset + new Vector2(rect.min.x, rect.max.y),
                offset + rect.max,
                offset + new Vector2(rect.max.x, rect.min.y), 
                color
            );
            
        /// <summary>
        /// 画方块.
        /// </summary>
        public static void Rect(Vector3 offset, Rect rect) => Rect(offset, rect, Color.white);
    }

}
