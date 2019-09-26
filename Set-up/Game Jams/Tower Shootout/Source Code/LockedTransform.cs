using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

[ExecuteAlways]
public class LockedTransform : MonoBehaviour
{
    [SerializeField]
    private bool _hide = true;

    void Update()
    {
        if (_hide)
            this.transform.hideFlags = HideFlags.HideInInspector;
        else
            this.transform.hideFlags = HideFlags.NotEditable;
    }
}