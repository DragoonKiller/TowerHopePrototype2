using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.UI
{
    [ExecuteAlways]
    public class MinimapCamera : MonoBehaviour
    {
        public RenderTexture outTexture;
        public Material postEffect;
        
        RenderTexture inTexture => this.GetComponent<Camera>().targetTexture;
        
        void OnPostRender()
        {
            Graphics.Blit(inTexture, outTexture, postEffect);
        }
    }

}
