using System;
using System.Collections;
using SmartData.SmartBool;
using UnityEngine;
using UnityEngine.Events;

//TODO Fix floating point errors
public class PlayerController : MonoBehaviour
{
    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    [SerializeField] private BoolReader _grounded;

    [SerializeField, LabelOverride("Rigidbody")]
    private Rigidbody2D _rb;

    [SerializeField, Disable] private bool _jumped = false;

    [Space]
    public float horizontalGroundedDeaccelerationSpeed = 2.5f;
    public float horizontalGroundedAccelerationSpeed = 5f;
    public float horizontalGroundedMaxSpeed = 10f;
    public float horizontalAerialDeaccelerationSpeed = 2.5f;
    public float horizontalAerialAccelerationSpeed = 5;
    public float horizontalAerialMaxSpeed = 10f;
    public float jumpThrust = 10f;

    private IEnumerator JumpedCoroutine()
    {
        _jumped = true;
        //TODO Replace this with something that actually makes sense...
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _jumped = false;
    }

    public void Jump()
    {
        if (!this.enabled) return;
        Jump(jumpThrust);
    }

    private void Jump(float thrust)
    {
        if (!_jumped)
        {
            if (_grounded.value)
            {
                //TODO Make this if an option
                if (_rb.velocity.y > 0) return;


                if (_rb.velocity.y < thrust)
                {
                    _rb.velocity += new Vector2(0, thrust);
                    if (_rb.velocity.y > thrust)
                        _rb.velocity = new Vector2(_rb.velocity.x, thrust);
                }

                StartCoroutine(JumpedCoroutine());
            }
        }
    }

    public void MoveLeft()
    {
        if (!this.enabled) return;
        if (_grounded.value)
            MoveLeft(horizontalGroundedAccelerationSpeed, horizontalGroundedMaxSpeed,
                horizontalGroundedDeaccelerationSpeed);
        else
            MoveLeft(horizontalAerialAccelerationSpeed, horizontalAerialMaxSpeed, horizontalAerialDeaccelerationSpeed);
    }

    public void MoveRight()
    {
        if (!this.enabled) return;
        if (_grounded.value)
            MoveRight(horizontalGroundedAccelerationSpeed, horizontalGroundedMaxSpeed,
                horizontalGroundedDeaccelerationSpeed);
        else
            MoveRight(horizontalAerialAccelerationSpeed, horizontalAerialMaxSpeed, horizontalAerialDeaccelerationSpeed);
    }

    private void MoveLeft(float acceleration, float maxSpeed, float deacceleration)
    {
        if (_rb.velocity.x < -maxSpeed) return;

        if (_rb.velocity.x > 0e-50f)
        {
            _rb.velocity -= new Vector2(deacceleration, 0);
            return;
        }

        if (_rb.velocity.x > -maxSpeed)
        {
            _rb.velocity -= new Vector2(acceleration, 0);
            if (_rb.velocity.x < -maxSpeed)
                _rb.velocity = new Vector2(-maxSpeed, _rb.velocity.y);
        }
    }

    private void MoveRight(float acceleration, float maxSpeed, float deacceleration)
    {
        if (_rb.velocity.x > maxSpeed) return;

        if (_rb.velocity.x < 0e-50f)
        {
            _rb.velocity += new Vector2(deacceleration, 0);
            return;
        }

        if (_rb.velocity.x < maxSpeed)
        {
            _rb.velocity += new Vector2(acceleration, 0);
            if (_rb.velocity.x > maxSpeed)
                _rb.velocity = new Vector2(maxSpeed, _rb.velocity.y);
        }
    }
}