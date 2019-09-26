using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [LabelOverride("Rigidbody"), SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private GroundedState _groundedState;
    public float gravity = 9.81f;
    public float terminalVelocity = 195f / 3.6f;


    void FixedUpdate()
    {
        if (_groundedState.IsGrounded()) return;
        if (_rb.velocity.y < -terminalVelocity) return;

        if (_rb.velocity.y > -terminalVelocity)
            _rb.velocity -= new Vector2(0, gravity);
        if (_rb.velocity.y < -terminalVelocity)
            _rb.velocity = new Vector2(_rb.velocity.x, -terminalVelocity);
    }
}