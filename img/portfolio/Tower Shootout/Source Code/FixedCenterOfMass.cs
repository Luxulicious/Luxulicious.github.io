using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FixedCenterOfMass : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField, LabelOverride("Center of Mass Position")]
    private Vector3 _center;

    void Start()
    {
        if (_rb == null)
            this._rb = this.GetComponent<Rigidbody2D>();
        _rb.centerOfMass = _center;
        //Debug.DrawLine(_rb.worldCenterOfMass, _rb.worldCenterOfMass + new Vector2(1, 1));
        //Debug.DrawLine(_rb.worldCenterOfMass, _rb.worldCenterOfMass + new Vector2(-1, 1));
        //Debug.DrawLine(_rb.worldCenterOfMass, _rb.worldCenterOfMass + new Vector2(-1, -1));
        //Debug.DrawLine(_rb.worldCenterOfMass, _rb.worldCenterOfMass + new Vector2(1, -1));
    }
}
