using UnityEngine;
using System.Runtime.CompilerServices;
using System;

namespace Utils
{
    public static class TransformExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform Clear(this Transform x)
        {
            x.localPosition = Vector3.zero;
            x.localScale = Vector3.one;
            x.localRotation = Quaternion.identity;
            return x;
        }
    }
}
