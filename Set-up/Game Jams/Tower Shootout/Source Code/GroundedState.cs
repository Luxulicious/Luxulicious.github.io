using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class OnGroundedStateEnterEvent : UnityEvent { }
[Serializable]
public class OnGroundedStateExitEvent : UnityEvent { }


public class GroundedState : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Used for determining the grounded state.")]
    private float minDegreeGrounded = -135;
    [SerializeField]
    [Tooltip("Used for determining the grounded state.")]
    private float maxDegreeGrounded = 135;
    [SerializeField, Util.ReadOnly]
    private bool _grounded;
    private bool _groundedPrev = false;
    [SerializeField, ReadOnly]
    private Dictionary<GameObject, Collision2D> _groundedCollisions = new Dictionary<GameObject, Collision2D>();
    [Tooltip("Latest observed normal of the object's standing.")]
    [HideInInspector]
    public Vector2 latestGroundedNormal;

    public OnGroundedStateEnterEvent onGroundedStateEnterEvent = new OnGroundedStateEnterEvent();
    public OnGroundedStateExitEvent onGroundedStateExitEvent = new OnGroundedStateExitEvent();

    public void FixedUpdate()
    {   
        _grounded = IsGrounded();
        if (!_groundedPrev && _grounded)
            onGroundedStateEnterEvent.Invoke();
        else if(_groundedPrev && !_grounded)
            onGroundedStateExitEvent.Invoke();
        _groundedPrev = _grounded;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        foreach (var contact in col.contacts)
        {
            if (NormalAnalysis.NormalInDegreeRange(contact.normal, minDegreeGrounded, maxDegreeGrounded))
            {
                latestGroundedNormal = contact.normal;
                if (this._groundedCollisions.ContainsKey(col.gameObject))
                {
                    this._groundedCollisions.Remove(col.gameObject);
                    this._groundedCollisions.Add(col.gameObject, col);
                }
                else
                {
                    this._groundedCollisions.Add(col.gameObject, col);
                }
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (this._groundedCollisions.ContainsKey(col.gameObject))
        {
            _groundedCollisions.Remove(col.gameObject);
        }
    }

    public bool IsGrounded()
    {
        var toRemoveKeys = new List<GameObject>();
        this._groundedCollisions.Keys.ToList().ForEach(x =>
        {
            if (x)
            {
                if (!x.activeSelf)
                {
                    toRemoveKeys.Add(x);
                }
            }
            else
            {
                if(!x)
                    toRemoveKeys.Add(x);
            }
        });
        toRemoveKeys.ForEach(x => _groundedCollisions.Remove(x));
        return this._groundedCollisions.Any();
    }
}

