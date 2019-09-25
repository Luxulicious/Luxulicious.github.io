using System;
using System.Collections;
using System.Collections.Generic;
using SmartData.SmartBool;
using UnityEngine;


public class NotGroundedAndIsCollidingWithImmovable : AndGateEventInvoker
{
    [Header("Required fields")] [SerializeField]
    private BoolReader _grounded;

    [Observe("CheckCollisionTypeDetection"), LabelOverride("Collision Type Detection")] [SerializeField]
    private Component _componentCollisionTypeDetection;

    private ICollisionTypeDetection _collisionTypeDetection;

    private void CheckCollisionTypeDetection()
    {
        var ctd = _componentCollisionTypeDetection as ICollisionTypeDetection;
        if (ctd == null)
            throw new Exception("Invalid type detection type");
        else
            _collisionTypeDetection = ctd;
    }

    void Awake()
    {
        CheckCollisionTypeDetection();
    }

    public override void Update()
    {
        base._continuousResult = !_grounded.value && _collisionTypeDetection.IsCollidingWithImmovable();
        base.Update();
    }

    public override void FixedUpdate()
    {
        base._continuousResult = !_grounded.value && _collisionTypeDetection.IsCollidingWithImmovable();
        base.FixedUpdate();
    }

    public void Invoke()
    {
        base.Invoke(!_grounded.value,
            _collisionTypeDetection.IsCollidingWithImmovable());
    }
}