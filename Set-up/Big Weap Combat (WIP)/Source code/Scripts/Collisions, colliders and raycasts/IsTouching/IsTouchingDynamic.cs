//TODO Optimize to not have to use list
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class UpdateTouchingDynamicEvent : UnityEvent<List<Collision2D>> { }
[Serializable]
public class IsTouchingDynamicEvent : UnityEvent<bool> { }

public class IsTouchingDynamic : MonoBehaviour
{
    [SerializeField]
    private IsTouchingDynamicEvent _isTouchingDynamicEvent = new IsTouchingDynamicEvent();
    [SerializeField]
    private UpdateTouchingDynamicEvent _updateTouchingDynamicEvent = new UpdateTouchingDynamicEvent();

    [SerializeField, ReadOnly]
    private List<Collision2D> _dynamicObjectCollisions = new List<Collision2D>();
    [SerializeField, ReadOnly]
    private Collider2D _latestCollisionCollider;

    void OnCollisionStay2D(Collision2D col)
    {
        if (IsDynamicObjectCollision(col))
        {
            AddToCollisions(col);
        }
    }

    void FixedUpdate()
    {
        _updateTouchingDynamicEvent.Invoke(_dynamicObjectCollisions);
        _isTouchingDynamicEvent.Invoke(_dynamicObjectCollisions.Any());
        _dynamicObjectCollisions = new List<Collision2D>();
    }

    private void RemoveFromCollisions(Collision2D col)
    {
        _dynamicObjectCollisions.RemoveAll(x => x.gameObject == col.gameObject);
    }

    private void AddToCollisions(Collision2D col)
    {
        if (_dynamicObjectCollisions.All(x => x.gameObject != col.gameObject))
        {
            _dynamicObjectCollisions.Add(col);
            _latestCollisionCollider = col.collider;
        }
        else
        {
            _dynamicObjectCollisions.RemoveAll(x => x.gameObject == col.gameObject);
            _dynamicObjectCollisions.Add(col);
            _latestCollisionCollider = col.collider;
        }
    }

    public static bool IsDynamicObjectCollision(Collision2D col)
    {
        var rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return false;
        if (!rb.isKinematic)
        {
            return true;
        }
        return false;
    }

    public static bool IsDynamicObjectCollider(Collider2D col)
    {
        if (col.isTrigger)
            return false;
        Rigidbody2D rb;
        try
        {
            rb = col.gameObject.GetComponent<Rigidbody2D>();
        }
        catch (Exception)
        {
            rb = null;
        }
        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                return true;
            }
        }
        return false;
    }
}
