using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class OnHitStunEvent : UnityEvent<int> { }

public class KnockbackReceiver : MonoBehaviour
{
    private bool enabled;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Collider2D _rbCollider;
    [SerializeField]
    private float _knockbackReducingModifier = 1f;
    [SerializeField]
    private float _hitstunModifier = 0.4f;
    [SerializeField]
    private OnHitStunEvent _onHitStunEvent = new OnHitStunEvent();
    [SerializeField]
    private Vector2 _groundedKnockbackDecay = new Vector2(1, 0);


    [SerializeField, LabelOverride("Wall bounciness")]
    private float _immovableObjectBounciness = 1;
    [Space, Header("Properties")]
    [SerializeField, ReadOnly]
    private bool _isBeingKnockedBack = false;
    [SerializeField, ReadOnly, LabelOverride("Frames in knockback")]
    private int _knockedBackFrames = 0;
    [SerializeField, ReadOnly, LabelOverride("Expected frames in knockback")]
    private int _expectedFramesInKnockback = 0;
    [SerializeField, ReadOnly, LabelOverride("Knockback velocity")]
    private Vector2 _knockbackVel;

    private void OnEnable()
    {
        enabled = true;
    }

    private void OnDisable()
    {
        enabled = false;
    }

    public void Knockback(Vector2 knockback)
    {
        if (!enabled) return;
        knockback = knockback / _knockbackReducingModifier;
        var knockbackMagnitude = knockback.magnitude;
        var expectedFramesInKnockback = Mathf.RoundToInt(knockbackMagnitude * _hitstunModifier);
        _onHitStunEvent.Invoke(expectedFramesInKnockback);
  
        if (_rb)
        {
            _rb.velocity = Vector2.zero;
            var rbMass = _rb.mass;
            _rb.velocity += knockback * rbMass;
            _knockbackVel = _rb.velocity;
        }
        else
        {
            throw new Exception("No rb or rb2d object detected!");
        }

        if (expectedFramesInKnockback > 0)
        {
            _isBeingKnockedBack = true;
            _expectedFramesInKnockback = expectedFramesInKnockback;
            _knockedBackFrames = 0;
            StopAllCoroutines();
            StartCoroutine(KnockbackDecayCoroutine(expectedFramesInKnockback));
        }
    }

    //TODO Refactor the duplicate code in this
    private void Knockback(Vector2 knockback, int framesLeft)
    {
        if (!enabled) return;
        _onHitStunEvent.Invoke(framesLeft);

        if (!_rb)
        {
            throw new Exception("No rb or rb2d object detected!");
        }
        if (_rb)
        {
            _rb.velocity = Vector2.zero;
            _rb.velocity += knockback;
            _knockbackVel = _rb.velocity;
        }

        if (framesLeft > 0)
        {
            _isBeingKnockedBack = true;
            _expectedFramesInKnockback = framesLeft;
            _knockedBackFrames = 0;
            StopAllCoroutines();
            StartCoroutine(KnockbackDecayCoroutine(framesLeft));
        }
    }

    private IEnumerator KnockbackDecayCoroutine(int frames)
    {
        /*//TODO Refactor this duplicate code in the if else
        /*if (_rb2DObject)
        {
            if (_rb2DObject.IsGrounded())
            {
                if (_rb2DObject.rb.velocity.x != 0)
                {
                    if (_rb2DObject.rb.velocity.x > 0)
                    {
                        _rb2DObject.rb.velocity -= new Vector2(_groundedKnockbackDecay.x, 0);
                        _rb2DObject.rb.velocity =
                            new Vector2(Mathf.Clamp(_rb2DObject.rb.velocity.x, 0, Single.PositiveInfinity),
                                _rb2DObject.rb.velocity.y);
                    }
                    else if (_rb2DObject.rb.velocity.x < 0)
                    {
                        _rb2DObject.rb.velocity += new Vector2(_groundedKnockbackDecay.x, 0);
                        _rb2DObject.rb.velocity =
                            new Vector2(Mathf.Clamp(_rb2DObject.rb.velocity.x, Single.NegativeInfinity, 0),
                                _rb2DObject.rb.velocity.y);
                    }
                }

                if (_rb2DObject.rb.velocity.y != 0)
                {
                    if (_rb2DObject.rb.velocity.y > 0)
                    {
                        _rb2DObject.rb.velocity -= new Vector2(_groundedKnockbackDecay.y, 0);
                        _rb2DObject.rb.velocity =
                            new Vector2(Mathf.Clamp(_rb2DObject.rb.velocity.y, 0, Single.PositiveInfinity),
                                _rb2DObject.rb.velocity.y);
                    }
                    else if (_rb2DObject.rb.velocity.y < 0)
                    {
                        _rb2DObject.rb.velocity += new Vector2(_groundedKnockbackDecay.y, 0);
                        _rb2DObject.rb.velocity =
                            new Vector2(Mathf.Clamp(_rb2DObject.rb.velocity.y, Single.NegativeInfinity, 0),
                                _rb2DObject.rb.velocity.y);
                    }
                }
            }#1#
        }
        else if (_rb)
        {
            //TODO Check for ground state first when doing x decay (Commented out code below is a start)
            /*
            var boundsExtentsRb = _rbCollider.bounds.extents;
            var boundsSize = _rbCollider.bounds.size;
            var boundsWidth = boundsSize.x;
            var boundsExtentsWidth = boundsExtentsRb.x;
            var rayCount = 12;
            var widthRayPadding = boundsWidth / (rayCount - 1);

            var hits =

            var raycastHit2D = Physics2D.Raycast(_rb.position - Vector2.up, -Vector2.up, Mathf.Infinity);
            if (raycastHit2D)
            {

            }
            #1#
            //TODO Uncomment this code once ground state is checked
            /*
            if (_rb.velocity.x != 0)
            {
                if (_rb.velocity.x > 0)
                {
                    _rb.velocity -= new Vector2(_groundedKnockbackDecay.x, 0);
                    _rb.velocity =
                        new Vector2(Mathf.Clamp(_rb.velocity.x, 0, Single.PositiveInfinity),
                            _rb.velocity.y);
                }
                if (_rb.velocity.x < 0)
                {
                    _rb.velocity += new Vector2(_groundedKnockbackDecay.x, 0);
                    _rb.velocity =
                        new Vector2(Mathf.Clamp(_rb.velocity.x, Single.NegativeInfinity, 0),
                            _rb.velocity.y);
                }
            }
            if (_rb.velocity.y != 0)
            {
                if (_rb.velocity.y > 0)
                {
                    _rb.velocity -= new Vector2(_groundedKnockbackDecay.y, 0);
                    _rb.velocity =
                        new Vector2(Mathf.Clamp(_rb.velocity.y, 0, Single.PositiveInfinity),
                            _rb.velocity.y);
                }
                else if (_rb.velocity.y < 0)
                {
                    _rb.velocity += new Vector2(_groundedKnockbackDecay.y, 0);
                    _rb.velocity =
                        new Vector2(Mathf.Clamp(_rb.velocity.y, Single.NegativeInfinity, 0),
                            _rb.velocity.y);
                }
            }
            #1#
        }*/

        _knockedBackFrames += 1;
        yield return new WaitForSeconds(1 / 60f * frames);
        _isBeingKnockedBack = false;
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsTouchingImmovable.IsImmovableObjectCollision(col)
            && _immovableObjectBounciness > 0
            && _isBeingKnockedBack)
        {
            var newDir = (Vector2.Reflect(_knockbackVel.normalized, col.contacts.First().normal)).normalized;
            Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + newDir * 5, Color.blue, Mathf.Infinity);
            var newKnockback = _immovableObjectBounciness * _knockbackVel.magnitude * newDir;
            Knockback(newKnockback, _expectedFramesInKnockback - _knockedBackFrames);
        }
    }
}
