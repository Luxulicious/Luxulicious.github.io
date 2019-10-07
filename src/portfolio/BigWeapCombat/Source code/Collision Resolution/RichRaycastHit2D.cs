using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class RichRaycastHit2D
{
    public RaycastHit2D hit;
    public Vector2 fromPos;
    public Vector2 dir;
    [CanBeNull] public GameObject fromGO;
    [CanBeNull] public Collider2D fromCol;
    [CanBeNull] public Rigidbody2D fromRb;

    public RichRaycastHit2D(RaycastHit2D hit, Vector2 fromPos, Vector2 dir)
    {
        this.hit = hit;
        this.fromPos = fromPos;
        this.dir = dir;
    }
}