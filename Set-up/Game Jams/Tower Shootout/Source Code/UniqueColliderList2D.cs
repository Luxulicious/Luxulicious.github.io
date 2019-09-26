using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO Make this an extensions of colliders
[Serializable]
public class UniqueColliderList2D
{
    private List<Collider2D> _colliders = new List<Collider2D>();

    public List<Collider2D> Collisions
    {
        get { return _colliders; }
        set { _colliders = value; /*TODO Check for uniqueness here!*/ }
    }

    public void Add(Collider2D col)
    {
        if (_colliders.Contains(col))
            _colliders.RemoveAll(x => Same(x, col));
        _colliders.Add(col);
    }

    public void AddRange(List<Collider2D> cols)
    {
        cols.ForEach(Add);
    }

    public bool Contains(Collider2D col)
    {
        return _colliders.Any(x => Same(x, col));
    }


    public bool ContainsAny(List<Collider2D> cols)
    {
        foreach (var collider in _colliders)
        {
            foreach (var col in cols)
            {
                if (Same(col, collider))
                    return true;
            }
        }

        return false;
    }

    public static bool Same(Collider2D colA, Collider2D colB)
    {
        return colA.gameObject == colB.gameObject && colA.gameObject == colB.gameObject;
    }

    public void RemoveAll(List<Collider2D> collidersDropper)
    {
        collidersDropper.ForEach(x => _colliders.RemoveAll(y => Same(x, y)));
    }
}