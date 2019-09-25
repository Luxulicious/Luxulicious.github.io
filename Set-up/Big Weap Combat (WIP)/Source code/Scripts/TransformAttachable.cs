using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class TransformAttachable : MonoBehaviour
{
    [Tooltip("Transform that will be adopted by the new parent"), SerializeField]
    private Transform _attachable;
    [Tooltip("The original parent of the attachable"), SerializeField]
    private Transform _originalParentAttachable;
    [SerializeField, Disable]
    private Transform _newParentAttachable;

    void Awake()
    {
        if (!_attachable)
            _attachable = this.transform;
        if (!_originalParentAttachable)
            _originalParentAttachable = _attachable.transform.parent;
    }

    public void Attach(Transform parent)
    {
        this._attachable.SetParent(parent);
        _newParentAttachable = parent;
    }

    public void Detach()
    {
        this._attachable.SetParent(_originalParentAttachable ? _originalParentAttachable : null);
        _newParentAttachable = null;
    }

    public Transform Attachable
    {
        get { return _attachable; }
    }
}