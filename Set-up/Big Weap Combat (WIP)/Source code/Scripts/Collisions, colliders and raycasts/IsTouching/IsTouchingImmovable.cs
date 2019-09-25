//TODO Optimize to not have to use list
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class UpdateTouchingImmovableEvent : UnityEvent<List<Collision2D>> { }
[Serializable]
public class IsTouchingImmovableEvent : UnityEvent<bool> { }


public class IsTouchingImmovable : MonoBehaviour
{

    [SerializeField]
    private IsTouchingImmovableEvent _isTouchingImmovableEvent = new IsTouchingImmovableEvent();

    [SerializeField]
    private UpdateTouchingImmovableEvent _updateTouchingImmovableEvent = new UpdateTouchingImmovableEvent();

    [SerializeField, ReadOnly] private List<Collision2D> _immovableObjectCollisions = new List<Collision2D>();
    [SerializeField, ReadOnly] private Collider2D _latestCollisionCollider;

    void OnCollisionStay2D(Collision2D col)
    {
        if (IsImmovableObjectCollision(col))
        {
            AddToCollisions(col);
        }
    }

    void FixedUpdate()
    {
        _updateTouchingImmovableEvent.Invoke(_immovableObjectCollisions);
        _isTouchingImmovableEvent.Invoke(_immovableObjectCollisions.Any());
        _immovableObjectCollisions = new List<Collision2D>();
    }

    private void RemoveFromCollisions(Collision2D col)
    {
        _immovableObjectCollisions.RemoveAll(x => x.gameObject == col.gameObject);
    }

    private void AddToCollisions(Collision2D col)
    {
        if (_immovableObjectCollisions.All(x => x.gameObject != col.gameObject))
        {
            _immovableObjectCollisions.Add(col);
            _latestCollisionCollider = col.collider;
        }
        else
        {
            _immovableObjectCollisions.RemoveAll(x => x.gameObject == col.gameObject);
            _immovableObjectCollisions.Add(col);
            _latestCollisionCollider = col.collider;
        }
    }

    public static bool IsImmovableObjectCollision(Collision2D col)
    {
        var rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (LayerMask.LayerToName(col.gameObject.layer).Contains(immovableObjectLayerName))
        {
            return true;
        }

        if (rb != null)
        {
            if (rb.isKinematic)
            {
                return true;
            }
        }
        else
        {
            return true;
        }

        return false;
    }

    public static bool IsImmovableObjectCollider(Collider2D col)
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
        if (LayerMask.LayerToName(col.gameObject.layer).Contains(immovableObjectLayerName))
            return true;
        if (rb != null)
        {
            if (rb.isKinematic)
            {
                return true;
            }
        }
        else
        {
            if (!col.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    public static LayerMask GetImmovableObjectLayerMask()
    {
        return LayerMask.GetMask(immovableObjectLayerName);
    }

    public static string immovableObjectLayerName = "Platform";
}