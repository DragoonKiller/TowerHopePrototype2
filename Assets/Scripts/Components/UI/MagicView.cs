using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tower.UI
{
    using Systems;
    using Utils;
    using Tower.Components;

    [RequireComponent(typeof(RectTransform))]
    public class MagicView : MonoBehaviour
    {
        [Tooltip("每个小图像的 prefab.")]
        public GameObject imageSource;

        [Tooltip("这个 Magic View 应当显示哪个角色的魔法值.")]
        public Role role;

        [Tooltip("每个用于显示魔法数值的图像需要间隔多远.")]
        public float seperationSpace;

        [Tooltip("当该点所示的魔法值为 1 时, 应当使用的颜色.")]
        public Color filledColor;

        [Tooltip("当该点所示的魔法值处于 0 到 1 之间, 应当使用的颜色.")]
        public Color fillingColor;

        [Tooltip("当该点所示的魔法值为 0 时, 应当使用的颜色.")]
        public Color emptyColor;

        RectTransform rect => this.GetComponent<RectTransform>();

        void Update()
        {
            if(role == null || role.magic == null)
            {
                Images(0); // 会清除所有图标.
                return;
            }

            // 添加一个微小的校正, 保证不出现一个整数 a 被 floor 到 a-1 的情况.
            float maxMagic = role.magic.maxMagic + Maths.eps;
            // 当前魔法值.
            float curMagic = role.magic.magic;
            // 需要显示多少个法力数值.
            int count = maxMagic.FloorToInt();

            var images = Images(count);
            float totalWidth = rect.rect.width;
            float totalWidthNoSpace = totalWidth - (count - 1) * seperationSpace;
            float singleWidthNoSpace = totalWidthNoSpace / count;
            for(int i = 0; i < count; i++)
            {
                // 改变 Image 位置和大小.
                float left = i * (singleWidthNoSpace + seperationSpace) - rect.sizeDelta.x * 0.5f;
                float right = left + singleWidthNoSpace - rect.sizeDelta.y * 0.5f;
                float top = rect.rect.yMax;
                float bottom = rect.rect.yMin;
                var position = new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f);
                var size = new Vector2((right - left), (top - bottom));
                var subrect = images[i].GetComponent<RectTransform>();
                subrect.localPosition = position;
                subrect.sizeDelta = size;
                
                // 当前点是表示哪个区间的魔法值状态.
                var fromMagic = i;
                var toMagic = i + 1;
                
                // 改变 image 颜色.
                images[i].color = 
                    curMagic < fromMagic ? emptyColor :
                    curMagic < toMagic ? fillingColor : 
                    filledColor;
                    
            }
        }

        /// <summary>
        /// 增加或删除该对象下的 Image, 使得其和要求的个数相等.
        /// </summary>
        List<Image> Images(int requiredCount)
        {
            var images = new List<Image>(transform.GetComponentsInChildren<Image>());

            while(images.Count > requiredCount)
            {
                DestroyImmediate(images.Last().gameObject);
                images.RemoveLast();
            }

            while(images.Count < requiredCount)
            {
                var x = Instantiate(imageSource, this.transform);
                images.Add(x.GetComponent<Image>());
            }

            return images;
        }
    }
}
