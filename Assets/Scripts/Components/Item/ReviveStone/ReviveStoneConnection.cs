using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    using Utils;
    
    /// <summary>
    /// 控制复活石之间的连接的绘制.
    /// 假设其原始朝向的"正前方"是向上, 即 Vector.up.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ReviveStoneConnection : MonoBehaviour
    {
        [Tooltip("标准缩放大小.这个大小用于确定箭头的uv坐标.脚本会根据transform.localScale和该数值算出应该将uv缩放到多少.")]
        public Vector2 baseScale;
        
        SpriteRenderer rd => this.GetComponent<SpriteRenderer>();
        
        void Update()
        {
            var mat = rd.material;
            var rate = this.transform.localScale.ToVec2() / baseScale;
            mat.SetVector("_UVScale", rate);
        }
        
        /// <summary>
        /// 把这个箭头的首位设置到给定点.
        /// 该函数假设物体图片的锚点即物体的位置, 且在图片中央.
        /// </summary>
        public void SetFromTo(Vector2 from, Vector2 to)
        {
            this.transform.position = ((from + to) * 0.5f).ToVec3(this.transform.position.z);
            this.transform.rotation = Quaternion.FromToRotation(Vector2.up, from.To(to));
            var spriteHeight = rd.sprite.texture.height / rd.sprite.pixelsPerUnit;
            this.transform.localScale = new Vector2(baseScale.x, spriteHeight * from.To(to).magnitude);
        }
    }

}
