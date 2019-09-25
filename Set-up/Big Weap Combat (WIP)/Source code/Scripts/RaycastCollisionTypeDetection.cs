using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SmartData.SmartBool;
using UnityEngine;

public class RaycastCollisionTypeDetection : CollisionTypeDetection<RaycastHit2D>
{
    [Serializable]
    public class CollisionTypes : CollisionTypes<RaycastHit2D>
    {
        public override bool IsImmovableObjectCollision(RaycastHit2D hit)
        {
            return IsObjectCollision(_layerMaskImmovable, hit);
        }

        public override bool IsDynamicObjectCollision(RaycastHit2D hit)
        {
            return IsObjectCollision(_layerMaskImmovable, hit);
        }

        protected override bool IsObjectCollision(LayerMask layerMask, RaycastHit2D hit)
        {
            return !hit.collider.isTrigger &&
                   layerMask.IsInLayerMask(hit.collider.gameObject);
        }
    }

    private bool _lateFixedUpdateCouroutineIsRunning;

    [SerializeField] private CollisionTypes _collisionTypes;

    [SerializeField] private BoolWriter _isCollidingWithImmovable;
    [SerializeField] private BoolWriter _isCollidingWithDynamic;

    private void Awake()
    {
        if (_isCollidingWithImmovable == null)
            Debug.LogError("IsCollidingWithImmovable is not set");
        if (_isCollidingWithDynamic == null)
            Debug.LogError("IsCollidingWithDynamic is not set");
    }

    public IEnumerator LateFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        if (_isCollidingWithImmovable != null)
            _isCollidingWithImmovable.value = false;
        if (_isCollidingWithDynamic != null)
            _isCollidingWithDynamic.value = false;
        _lateFixedUpdateCouroutineIsRunning = false;
    }

    public void OnHitListener(RaycastHit2D hit)
    {
        if (_isCollidingWithImmovable != null)
            _isCollidingWithImmovable.value = _collisionTypes.IsImmovableObjectCollision(hit);
        if (_isCollidingWithDynamic != null)
            _isCollidingWithDynamic.value = _collisionTypes.IsDynamicObjectCollision(hit);
        EnableLateFixedUpdate();
    }

    public void OnHitImmovableListener(RaycastHit2D hit)
    {
        var result = _collisionTypes.IsImmovableObjectCollision(hit);
        if (!result)
            throw new Exception("Invalid immmovable layermask raycast hit");
        _isCollidingWithImmovable.value = result;
        EnableLateFixedUpdate();
    }
    
    public void OnHitDynamicListener(RaycastHit2D hit)
    {
        throw new NotImplementedException();
        var result = _collisionTypes.IsDynamicObjectCollision(hit);
        if (!result)
            throw new Exception("Invalid dynamic layermask raycast hit");
        if (_isCollidingWithDynamic != null)
            _isCollidingWithDynamic.value = result;
        StartCoroutine(LateFixedUpdate());
    }

    private void EnableLateFixedUpdate()
    {
        if (!_lateFixedUpdateCouroutineIsRunning)
        {
            _lateFixedUpdateCouroutineIsRunning = true;
            StartCoroutine(LateFixedUpdate());
        }
    }

    public override bool IsCollidingWithImmovable()
    {
        if (_isCollidingWithImmovable != null)
            return _isCollidingWithImmovable.value;
        return false;
    }

    public override bool IsCollidingWithDynamic()
    {
        if (_isCollidingWithDynamic != null)
            return _isCollidingWithImmovable.value;
        return false;
    }
}