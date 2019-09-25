using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class NormalAnalysis
{
    [Obsolete("Use Vector2.VectorInDegreeRange instead", false)]
    public static bool NormalInDegreeRange(Vector2 normal, float minDegree, float maxDegree)
    {
        minDegree = -minDegree;
        var minY = 0;
        var minX = Mathf.Cos((Mathf.PI / 180) * minDegree);
        var maxX = -Mathf.Cos((Mathf.PI / 180) * maxDegree);
        return normal.y >= minY
               && normal.x >= minX
               && normal.x <= maxX;
    }
}