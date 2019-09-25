using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Vector2Extensions
{
    public static bool Equals(Vector2 vector,  Vector2 other, float tolerance)
    {
        return Math.Abs(vector.x - other.x) < tolerance && Math.Abs(vector.y - other.y) < tolerance;
    }

    public static bool Equals(this Vector2 vector, Vector2 a, Vector2 b, float tolerance)
    {
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance;
    }

    public static Vector2 Clamp(this Vector2 v, float minX, float maxX, float minY, float maxY)
    {
        v = new Vector2(
            Mathf.Clamp(v.x, minX, maxX),
            Mathf.Clamp(v.y, minY, maxX)
        );
        return v;
    }

    public static bool VectorInDegreeRange(this Vector2 v, float minDegree, float maxDegree)
    {
        minDegree = -minDegree;
        var minY = 0;
        var minX = Mathf.Cos((Mathf.PI / 180) * minDegree);
        var maxX = -Mathf.Cos((Mathf.PI / 180) * maxDegree);
        return v.y >= minY
               && v.x >= minX
               && v.x <= maxX;
    }
}