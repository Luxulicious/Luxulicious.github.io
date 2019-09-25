using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

//TODO Make this an extensions of colliders
//TODO Write custom inspector / serialization logic for Unity's Editor
[Serializable]
public class UniqueCollisionList2D
{
    private List<Collision2D> _collisions = new List<Collision2D>();

    public List<Collision2D> Collisions
    {
        get { return _collisions; }
        set { _collisions = value; /*TODO Check for uniqueness here!*/ }
    }

    public void Add(Collision2D col)
    {
        if (_collisions.Contains(col))
            _collisions.RemoveAll(x => Same(x, col));
        _collisions.Add(col);
    }

    public bool Contains(Collision2D col)
    {
        return _collisions.Any(x => Same(x, col));
    }

    public static bool Same(Collision2D colA, Collision2D colB)
    {
        return colA.gameObject == colB.gameObject && colA.otherCollider.gameObject == colB.otherCollider.gameObject;
    }
}
