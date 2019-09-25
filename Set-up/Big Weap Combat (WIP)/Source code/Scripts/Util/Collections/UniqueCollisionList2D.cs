using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Contains a unique list of Collision2D with the pair of game objects
/// belonging to each collider involved in the collision as the unique
/// constraint.
/// </summary>
//TODO Write a custom inspector
[Serializable]
public class UniqueCollisionList2D
{
    [SerializeField] private List<Collision2D> _collisions = new List<Collision2D>();

    public List<Collision2D> Collisions
    {
        get { return _collisions; }
        set
        {
            _collisions = value;
            /*TODO Check for uniqueness here!*/
            throw new NotImplementedException();
        }
    }

    public bool Any()
    {
        return _collisions.Any();
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

    public bool Remove(Collision2D col)
    {
        return _collisions.RemoveAll(x => Same(x, col)) > 0;
    }

    public static bool Same(Collision2D colA, Collision2D colB)
    {
        return colA.gameObject == colB.gameObject && colA.otherCollider.gameObject == colB.otherCollider.gameObject;
    }
}