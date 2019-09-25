using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[ExecuteAlways]
public class Mass : MonoBehaviour
{
    [SerializeField, Observe("GetMassFromRb")]
    private Rigidbody2D _rb;
    [Range(0.0001f, 1000000), Observe("SetRbMassFromMass")]
    public float mass;

    void Awake()
    {
        CheckNullBody();     
    }

    void Start()
    {
        
        //Debug.LogWarning("TODO: Undo registering does not work while trying to build.");
        //Undo.undoRedoPerformed += SetRbMassFromMass;
    }

    private void CheckNullBody()
    {
        if (!_rb)
        {
            _rb = this.GetComponent<Rigidbody2D>();
            GetMassFromRb();
        }
    }

    private void GetMassFromRb()
    {
        if (!_rb) return;
        mass = _rb.mass;
    }

    private void SetRbMassFromMass()
    {
        if (!_rb) return;
        _rb.mass = mass;
    }
}