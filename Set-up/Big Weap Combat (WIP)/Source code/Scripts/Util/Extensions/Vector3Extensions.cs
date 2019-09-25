using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Clamp(this Vector3 v, float minX, float maxX, float minY, float maxY)
    {
        v = new Vector3(
            Mathf.Clamp(v.x, minX, maxX),
            Mathf.Clamp(v.y, minY, maxY),
            v.z
        );
        return v;
    }
}