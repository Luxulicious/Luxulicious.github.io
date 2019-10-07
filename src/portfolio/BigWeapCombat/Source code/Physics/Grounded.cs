using System.Collections.Generic;
using System.Linq;
using SmartData.SmartBool;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] [Tooltip("Used for determining the grounded state.")]
    private float minDegreeGrounded = -135;

    [SerializeField] [Tooltip("Used for determining the grounded state.")]
    private float maxDegreeGrounded = 135;

    [SerializeField] private BoolWriter _grounded;
    [SerializeField, Disable] private bool _isGrounded;
    [SerializeField, Disable] private Vector2 _latestGroundedNormal;

    [SerializeField, Disable]
    private Dictionary<GameObject, Collision2D> _groundedCollisions = new Dictionary<GameObject, Collision2D>();

    void FixedUpdate()
    {
        _grounded.value = IsGrounded();
        _isGrounded = _grounded.value;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        foreach (var contact in col.contacts)
        {
            if (contact.normal.VectorInDegreeRange(minDegreeGrounded, maxDegreeGrounded))
            {
                _latestGroundedNormal = contact.normal;
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

    private bool IsGrounded()
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
                if (!x)
                    toRemoveKeys.Add(x);
            }
        });
        toRemoveKeys.ForEach(x => _groundedCollisions.Remove(x));
        return this._groundedCollisions.Any();
    }
}