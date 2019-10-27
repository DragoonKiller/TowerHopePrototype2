using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Components
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraSyncTexture : MonoBehaviour
    {
        public Camera cam;
        public RenderTexture target;
        
        void Update()
        {
            if(target.width != cam.pixelWidth || target.height != cam.pixelHeight)
            {
                target.Release();
                target.width = cam.pixelWidth;
                target.height = cam.pixelHeight;
                target.Create();
            }    
        }
    }
}
