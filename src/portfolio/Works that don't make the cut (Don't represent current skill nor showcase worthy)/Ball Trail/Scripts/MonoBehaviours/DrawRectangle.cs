using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
    public Color color = Color.magenta;
    public Rectangle rectangle;

    void Start()
    {
        if (!rectangle)
            this.rectangle = this.GetComponent<Rectangle>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        var points = rectangle.GetPoints();
        if (points.Count <= 1) return;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
        Gizmos.DrawLine(points[points.Count - 1], points[0]);
    }
}
