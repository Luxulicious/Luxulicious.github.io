using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Vector2Extensions
{
    public static bool Equals(this Vector2 vector, Vector2 a, Vector2 b, float tolerance)
    {
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance;
    }
}