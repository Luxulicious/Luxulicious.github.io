using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ICollisionTypeDetection))]
public class CompositeCollisionTypeDetection : MonoBehaviour, ICollisionTypeDetection
{
    [Observe("CheckCollisionTypeDetection"), LabelOverride("Collision Type Detections")]
    [SerializeField]
    private List<Component> _componentCollisionTypeDetections = new List<Component>();
    private List<ICollisionTypeDetection> _collisionTypeDetections = new List<ICollisionTypeDetection>();

    private void CheckCollisionTypeDetection()
    {
        _collisionTypeDetections.Clear();
        _componentCollisionTypeDetections.ForEach(x =>
        {
            var ctd = x as ICollisionTypeDetection;
            if (ctd == null)
                throw new Exception("Invalid type detection type");
            else
                _collisionTypeDetections.Add(ctd);
        });
    }

    void Awake()
    {
        CheckCollisionTypeDetection();
    }

    public bool IsCollidingWithImmovable()
    {
        var result = false;
        foreach (var x in _collisionTypeDetections)
        {
            if (x.IsCollidingWithImmovable())
                result = true;
        }

        return result;
    }

    public bool IsCollidingWithDynamic()
    {
        throw new System.NotImplementedException();
    }
}