using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直接跟踪一个物体.
/// </summary>
[ExecuteAlways]
public class CameraFixFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    
    void Update()
    {
        this.transform.position = target.position + offset;
    }
}
