using System;
using UnityEngine;

[Serializable]
public struct ScaleSettings
{
    public Vector2 minScale;
    public Vector2 maxScale;
    public Vector2 scaleUpSpeed;
    public Vector2 scaleDownSpeed;
}