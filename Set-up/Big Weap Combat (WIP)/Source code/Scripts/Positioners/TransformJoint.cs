using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes transform act like a joint (accounting for properties checked in class )
/// </summary>
public class TransformJoint : MonoBehaviour
{
    [SerializeField]
    private Transform _pos;
    [SerializeField]
    private Transform _scale;
    [SerializeField]
    private bool _positionX;
    [SerializeField]
    private bool _positionY;
    [SerializeField]
    private bool _positionZ;
    //TODO Rotation inheritance
    //[SerializeField]
    //private bool _rotationX;
    //[SerializeField]
    //private bool _rotationY;
    //[SerializeField]
    //private bool _rotationZ;
    [SerializeField]
    private bool _scaleX;
    [SerializeField]
    private bool _scaleY;
    [SerializeField]
    private bool _scaleZ;


    void FixedUpdate()
    {
        InheritPosition();
        InheritScale();
    }

    public void InheritTransform()
    {
        InheritPosition();
        InheritTransform();
    }

    private void InheritPosition()
    {
        if (_pos != null)
        {
            if (_positionX)
                this.transform.position =
                    new Vector3(_pos.position.x, this.transform.position.y, this.transform.position.z);
            if (_positionY)
                this.transform.position =
                    new Vector3(this.transform.position.x, _pos.position.y, this.transform.position.z);
            if (_positionZ)
                this.transform.position =
                    new Vector3(this.transform.position.x, this.transform.position.y, _pos.position.z);
        }
    }

    private void InheritScale()
    {
        if (_scale != null)
        {
            if (_scaleX)
                this.transform.localScale = new Vector3(_scale.localScale.x, this.transform.localScale.y,
                    this.transform.localScale.z);
            if (_scaleY)
                this.transform.localScale = new Vector3(this.transform.localScale.x, _scale.localScale.y,
                    this.transform.localScale.z);
            if (_scaleZ)
                this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y,
                    _scale.localScale.z);
        }
    }
}
