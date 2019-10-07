using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HorizontalDrag : MonoBehaviour
{
    [SerializeField, LabelOverride("Rigidbody")]
    private  Rigidbody2D _rb;
    public float drag = 5f;

    protected void FixedUpdate()
    {
        if (Math.Abs(_rb.velocity.x) > 0.000f)
        {
            if (_rb.velocity.x > 0)
            {
                _rb.velocity -= new Vector2(drag, 0);
                _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, 0, Mathf.Infinity), _rb.velocity.y);
            }
            else if (_rb.velocity.x < 0)
            {
                _rb.velocity += new Vector2(drag, 0);
                _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, Mathf.NegativeInfinity, 0), _rb.velocity.y);
            }
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }
}