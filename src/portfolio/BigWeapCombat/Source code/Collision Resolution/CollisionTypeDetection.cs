using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class CollisionTypeDetection<T> : MonoBehaviour, ICollisionTypeDetection
{
    [Serializable]
    public abstract class CollisionTypes<T>
    {
        //TODO Might need to replace layers with something else eventually...
        [SerializeField] protected LayerMask _layerMaskImmovable;
        [SerializeField] protected LayerMask _layerMaskDynamic;

        public abstract bool IsImmovableObjectCollision(T collision);

        public abstract bool IsDynamicObjectCollision(T collision);

        public LayerMask GetLayerMaskImmovable()
        {
            return _layerMaskImmovable;
        }

        public LayerMask GetLayerMaskDynamic()
        {
            return _layerMaskDynamic;
        }

        protected abstract bool IsObjectCollision(LayerMask layerMask, T collision);
    }

    public abstract bool IsCollidingWithImmovable();
    public abstract bool IsCollidingWithDynamic();
}