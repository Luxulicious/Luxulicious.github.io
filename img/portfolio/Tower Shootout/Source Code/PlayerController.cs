using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GroundedState _groundedState;
    [SerializeField, LabelOverride("Rigidbody")]
    private Rigidbody2D _rb;
    [SerializeField, Disable]
    private bool _jumped = false;

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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _jumped = false;
    }

    public void Jump()
    {
        Jump(jumpThrust);
    }

    private void Jump(float thrust)
    {
        if (!_jumped)
        {
            if (_groundedState.IsGrounded())
            {
                //TODO Make this if an option
                //if (_rb.velocity.y > 0) return;

                //if (_rb.velocity.y < thrust)
                //{
                    _rb.velocity += new Vector2(0, thrust);
                    if(_rb.velocity.y > thrust)
                        _rb.velocity = new Vector2(_rb.velocity.x, thrust);
                //}
                StartCoroutine(JumpedCoroutine());
            }
        }
    }

    public void MoveLeft()
    {
        if (_groundedState.IsGrounded())
            MoveLeft(horizontalGroundedAccelerationSpeed, horizontalGroundedMaxSpeed,
                     horizontalGroundedDeaccelerationSpeed);
        else
            MoveLeft(horizontalAerialAccelerationSpeed, horizontalAerialMaxSpeed, horizontalAerialDeaccelerationSpeed);
    }

    public void MoveRight()
    {
        if (_groundedState.IsGrounded())
            MoveRight(horizontalGroundedAccelerationSpeed, horizontalGroundedMaxSpeed,
                      horizontalGroundedDeaccelerationSpeed);
        else
            MoveRight(horizontalAerialAccelerationSpeed, horizontalAerialMaxSpeed, horizontalAerialDeaccelerationSpeed);
    }

    private void MoveLeft(float acceleration, float maxSpeed, float deacceleration)
    {
        if (_rb.velocity.x < -maxSpeed) return;

        if (_rb.velocity.x > 0)
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

        if (_rb.velocity.x < 0)
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


//using System;
//using UnityEngine;
//using UnityEngine.Events;

//[Serializable]
//public class OnJumpEvent : UnityEvent { }

//public class PlayerController : MonoBehaviour
//{
//    private bool _enabled;

//    [SerializeField]
//    private Rb2DObject rb2DObject;

//    [Space]
//    public float horizontalGroundedAccelerationSpeed = 5f;
//    public float horizontalGroundedMaxSpeed = 10f;
//    public float horizontalAerialAccelerationSpeed = 5;
//    public float horizontalAerialMaxSpeed = 10f;
//    public float jumpThrust = 10f;

//    [Space]
//    [SerializeField]
//    private OnJumpEvent _onJumpEvent = new OnJumpEvent();

//    private void OnEnable()
//    {
//        _enabled = true;
//    }

//    private void OnDisable()
//    {
//        _enabled = false;
//    }

//    // Use this for initialization
//    void Start()
//    {
//        if (this.rb2DObject == null)
//            this.rb2DObject = this.GetComponent<Rb2DObject>();
//    }

//    public void Jump()
//    {
//        if (!enabled) return;
//        Jump(jumpThrust);
//    }

//    public void Jump(float thrust)
//    {
//        if (!enabled) return;
//        if (rb2DObject.IsGrounded())
//        {
//            rb2DObject.eDec.y -= thrust;
//            _onJumpEvent.Invoke();
//        }
//    }

//    public void MoveLeft()
//    {
//        if (!enabled) return;
//        if (rb2DObject.IsGrounded())
//            MoveLeft(horizontalGroundedAccelerationSpeed);
//        else
//            MoveLeft(horizontalAerialAccelerationSpeed);
//    }

//    public void MoveRight()
//    {
//        if (!enabled) return;
//        if (rb2DObject.IsGrounded())
//            MoveRight(horizontalGroundedAccelerationSpeed);
//        else
//            MoveRight(horizontalAerialAccelerationSpeed);
//    }

//    public void MoveLeft(float speed)
//    {
//        if (!enabled) return;
//        if (rb2DObject.IsGrounded())
//        {
//            if (rb2DObject.rb.velocity.x + -speed > -horizontalGroundedMaxSpeed)
//                rb2DObject.eDec.x += speed;
//            else if (rb2DObject.rb.velocity.x > -horizontalGroundedMaxSpeed)
//                rb2DObject.eDec.x = horizontalGroundedMaxSpeed + rb2DObject.rb.velocity.x;
//        }
//        else
//        {
//            if (rb2DObject.rb.velocity.x + -speed > -horizontalAerialMaxSpeed)
//                rb2DObject.eDec.x += speed;
//            else if (rb2DObject.rb.velocity.x > -horizontalAerialMaxSpeed)
//                rb2DObject.eDec.x = horizontalAerialMaxSpeed + rb2DObject.rb.velocity.x;
//        }
//    }

//    public void MoveRight(float speed)
//    {
//        if (!enabled) return;
//        if (rb2DObject.IsGrounded())
//        {
//            if (rb2DObject.rb.velocity.x + speed < horizontalGroundedMaxSpeed)
//                rb2DObject.eDec.x -= speed;
//            else if (rb2DObject.rb.velocity.x < horizontalGroundedMaxSpeed)
//                rb2DObject.eDec.x = -horizontalGroundedMaxSpeed + rb2DObject.rb.velocity.x;
//        }
//        else
//        {
//            if (rb2DObject.rb.velocity.x + speed < horizontalAerialMaxSpeed)
//                rb2DObject.eDec.x -= speed;
//            else if (rb2DObject.rb.velocity.x < horizontalAerialMaxSpeed)
//                rb2DObject.eDec.x = -horizontalAerialMaxSpeed + rb2DObject.rb.velocity.x;
//        }
//    }
//}