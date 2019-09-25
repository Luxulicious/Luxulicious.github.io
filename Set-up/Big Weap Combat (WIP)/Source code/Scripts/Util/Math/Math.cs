using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public static class Polygon
{
    public static bool IsPointInPolygon(Vector2 p, Vector2[] polygon)
    {
        double minX = polygon[0].x;
        double maxX = polygon[0].x;
        double minY = polygon[0].y;
        double maxY = polygon[0].y;
        for (int i = 1; i < polygon.Length; i++)
        {
            Vector2 q = polygon[i];
            minX = Math.Min(q.x, minX);
            maxX = Math.Max(q.x, maxX);
            minY = Math.Min(q.y, minY);
            maxY = Math.Max(q.y, maxY);
        }

        if (p.x < minX || p.x > maxX || p.y < minY || p.y > maxY)
        {
            return false;
        }

        // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if ((polygon[i].y > p.y) != (polygon[j].y > p.y) &&
                p.x < (polygon[j].x - polygon[i].x) * (p.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    public static void DrawPolygon(Action<Vector3, Vector3> draw, Vector2 origin, List<Vector2> polygon, Color color)
    {
        Gizmos.color = color;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (i != polygon.Count - 1)
                draw(origin + polygon[i], origin + polygon[i + 1]);
            else
                draw(origin + polygon[i], origin + polygon[0]);
        }
    }
}

public static class Angle
{
    public static bool VectorWithinDegreeRange(Vector2 vec, float minDegree, float maxDegree)
    {
        if (minDegree > maxDegree)
            throw new Exception("Minimum degree cannot be higher than maximum degree.");
        var minRad = Mathf.Deg2Rad * minDegree;
        var maxRad = Mathf.Deg2Rad * maxDegree;
        return VectorWithinRadianRange(vec, minRad, maxRad);
    }

    public static bool VectorWithinRadianRange(Vector2 vec, float minRad, float maxRad)
    {
        if (minRad > maxRad)
            throw new Exception("Minimum degree cannot be higher than maximum degree.");
        var angle = Vector2ToRadian(vec);
        return
            angle >= minRad && angle <= maxRad;
    }

    public static float Vector2ToRadian(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x);
    }
}



