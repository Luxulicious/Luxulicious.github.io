using System;
using System.Collections.Generic;
using Assets.Main.Scripts.Util;
using SmartData.SmartBool;
using UnityEngine;

public class InheritVelocity : MonoBehaviour
{
    [Serializable]
    internal struct BoolXYPair
    {
        public bool x, y;
    }

    [LabelOverride("Parent Rigidbody")] [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private List<Rigidbody2D> _rbChildren = new List<Rigidbody2D>();

    [SerializeField] private BoolXYPair _inheritingVelocity;

    private void FixedUpdate()
    {
        if (_inheritingVelocity.x)
            _rbChildren.ForEach(rb => rb.velocity = new Vector2(_rb.velocity.x, rb.velocity.y));
        if (_inheritingVelocity.y)
            _rbChildren.ForEach(rb => rb.velocity = new Vector2(rb.velocity.x, _rb.velocity.y));
    }

    public void SetVelocities(Vector2 vel)
    {
        _rbChildren.ForEach(rb => rb.velocity = vel);
    }

    public void InheritVelocityOnce(bool inheritVelocity)
    {
        InheritVelocityXOnce(inheritVelocity);
        InheritVelocityYOnce(inheritVelocity);
    }

    public void InheritVelocityXOnce(bool inheritVelocityX)
    {
        _rbChildren.ForEach(x => x.velocity = new Vector2(_rb.velocity.x, x.velocity.y));
    }

    public void InheritVelocityYOnce(bool inheritVelocityX)
    {
        _rbChildren.ForEach(x => x.velocity = new Vector2(x.velocity.x, _rb.velocity.y));
    }


    public void StartInheritingVelocity()
    {
        SetInheritingVelocity(true);
    }

    public void EndInheritingVelocity()
    {
        SetInheritingVelocity(false);
    }


    public void SetInheritingVelocity(bool inheritVelocity)
    {
        SetInheritingVelocityX(inheritVelocity);
        SetInheritingVelocityY(inheritVelocity);
    }

    public void SetInheritingVelocityX(bool inheritVelocityX)
    {
        _inheritingVelocity.x = inheritVelocityX;
    }

    public void SetInheritingVelocityY(bool inheritVelocityY)
    {
        _inheritingVelocity.y = inheritVelocityY;
    }
}