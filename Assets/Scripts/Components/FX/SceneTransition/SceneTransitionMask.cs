using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tower.Components
{
    using Utils;
    
    public class SceneTransitionMask : MonoBehaviour
    {
        public SceneTransition trnasition;
        public Image targetImage;
        
        void Update()
        {
            targetImage.color = targetImage.color.A(trnasition.curtain);
        }
    }
}
