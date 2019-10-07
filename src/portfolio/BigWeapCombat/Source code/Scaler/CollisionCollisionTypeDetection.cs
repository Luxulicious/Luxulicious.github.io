using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using SmartData.SmartBool;
using UnityEngine;

public class CollisionCollisionTypeDetection : CollisionTypeDetection<Collision2D>
{
    //TODO These fields or the struct itself should be scriptable objects or singletons
    [Serializable]
    public struct CollisionTypes
    {
        //TODO Might need to replace layers with something else eventually...
        [SerializeField] private LayerMask _layerMaskImmovable;
        [SerializeField] private LayerMask _layerMaskDynamic;

        public bool IsImmovableObjectCollision(Collision2D collision)
        {
            return IsObjectCollision(_layerMaskImmovable, collision);
        }

        public bool IsDynamicObjectCollision(Collision2D collision)
        {
            return IsObjectCollision(_layerMaskImmovable, collision);
        }

        public LayerMask GetLayerMaskImmovable()
        {
            return _layerMaskImmovable;
        }

        public LayerMask GetLayerMaskDynamic()
        {
            return _layerMaskDynamic;
        }

        private bool IsObjectCollision(LayerMask layerMask, Collision2D collision)
        {
            return !collision.collider.isTrigger &&
                   layerMask.IsInLayerMask(collision.collider.gameObject);
        }
    }

    [Serializable]
    public struct ActiveCollisions
    {
        [Disable] public UniqueCollisionList2D immovableCollisions;
        [Disable] public UniqueCollisionList2D dynamicCollisions;
    }

    [SerializeField] private CollisionTypes _collisionTypes;
    [SerializeField] private ActiveCollisions _activeCollisions;
    [SerializeField] private BoolWriter _isCollidingWithImmovable;
    [SerializeField] private BoolWriter _isCollidingWithDynamic;

    void Awake()
    {
        if (_isCollidingWithImmovable == null)
            Debug.LogError("IsCollidingWithImmovable is not set");
        if (_isCollidingWithDynamic == null)
            Debug.LogError("IsCollidingWithDynamic is not set");
    }

    public void OnCollisionEnter2DListener(Collision2D collision)
    {
        if (_collisionTypes.IsImmovableObjectCollision(collision))
            _activeCollisions.immovableCollisions.Add(collision);
        else if (_collisionTypes.IsDynamicObjectCollision(collision))
            _activeCollisions.dynamicCollisions.Add(collision);

        if (_isCollidingWithImmovable != null)
            _isCollidingWithImmovable.value = IsCollidingWithImmovable();
        if (_isCollidingWithDynamic != null)
            _isCollidingWithDynamic.value = IsCollidingWithDynamic();
    }

    public void OnCollisionExit2DListener(Collision2D collision)
    {
        if (_collisionTypes.IsImmovableObjectCollision(collision))
            _activeCollisions.immovableCollisions.Remove(collision);
        else if (_collisionTypes.IsDynamicObjectCollision(collision))
            _activeCollisions.dynamicCollisions.Remove(collision);

        if (_isCollidingWithImmovable != null)
            _isCollidingWithImmovable.value = IsCollidingWithImmovable();
        if (_isCollidingWithDynamic != null)
            _isCollidingWithDynamic.value = IsCollidingWithDynamic();
    }

    public ActiveCollisions GetActiveCollisions()
    {
        return _activeCollisions;
    }

    public CollisionTypes GetCollisionTypes()
    {
        return _collisionTypes;
    }

    public override bool IsCollidingWithImmovable()
    {
        return _activeCollisions.immovableCollisions.Any();
    }

    public override bool IsCollidingWithDynamic()
    {
        return _activeCollisions.dynamicCollisions.Any();
    }
}