using UnityEngine;
using Util;

[RequireComponent(typeof(HingeJoint2D))]
public class FixedHingeJoint : MonoBehaviour
{
    [SerializeField, ReadOnly, LabelOverride("Original Position")]
    private Vector2 _oPos;

    void Start()
    {
        _oPos = this.transform.localPosition;
    }

    void FixedUpdate()
    {
        this.transform.localPosition = _oPos;
    }
}
