using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class OnCollisionEnterAttachableTransformEvent : UnityEvent<Transform> { }

[Serializable]
public class OnCollisionExitAttachableTransformEvent : UnityEvent<Transform> { }

public class AttachToTransform : MonoBehaviour, IDisablable
{
    protected internal static string errorMsgFaultyCollisionDetection =
        "Can't detect manually while in manual collision detection mode.";


    [SerializeField]
    private Transform _attachToTransform;
    [Tooltip("Makes attaching not happen automatically from this game object's colliders."), SerializeField]
    private bool _manualCollisionDetection;
    [SerializeField, ReadOnly]
    private List<TransformAttachable> _currentTransformAttachables = new List<TransformAttachable>();

    void Start()
    {
        if (!_attachToTransform)
            _attachToTransform = this.transform;
    }

    public void OnCollisionEnter2DEventHandler(Collision2D other)
    {
        if (!_manualCollisionDetection) throw new Exception(errorMsgFaultyCollisionDetection);
        var transformAttachable = other.transform.GetComponent<TransformAttachable>();
        if (!transformAttachable) return;

        if (!_currentTransformAttachables.Contains(transformAttachable))
        {
            _currentTransformAttachables.Add(transformAttachable);
            transformAttachable.Attach(_attachToTransform);
        }
    }

    public void OnCollisionExit2DEventHandler(Collision2D other)
    {
        if (!_manualCollisionDetection) throw new Exception(errorMsgFaultyCollisionDetection);
        var transformAttachable = other.transform.GetComponent<TransformAttachable>();
        if (!transformAttachable) return;

        if (_currentTransformAttachables.Contains(transformAttachable))
        {
            /*
             * In case of a transfer of a other transform attachable with the same target attachable,
             * then don't detach the transform attachable just yet.
             * And if they are the last remaining transform attachable with that target then do detach.
            */
            var count = _currentTransformAttachables.Count(x => x.Attachable == transformAttachable.Attachable);
            if (count <= 1)
                transformAttachable.Detach();
            _currentTransformAttachables.Remove(transformAttachable);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!_manualCollisionDetection)
            OnCollisionEnter2DEventHandler(other);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!_manualCollisionDetection)
            OnCollisionExit2DEventHandler(other);
    }

    public void OnBeforeDisable()
    {
        _attachToTransform.gameObject.SetActive(true);
        _currentTransformAttachables.ForEach(x => x.Detach());
        _currentTransformAttachables.Clear();
        _attachToTransform.gameObject.SetActive(false);
    }
}