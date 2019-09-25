using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the angle of knockback based on the rotation of a transform
/// </summary>
public class HitboxAngler : MonoBehaviour
{
    [SerializeField]
    private Hitbox _hitbox;
    [SerializeField]
    private Transform _transformAngle;

    void FixedUpdate()
    {
        if (_hitbox.enabled)
            if (_transformAngle)
                //TODO Maybe change this to vector instead for consistency
                _hitbox.knockbackAngleInDegrees = _transformAngle.localEulerAngles.z + 90;
    }
}
