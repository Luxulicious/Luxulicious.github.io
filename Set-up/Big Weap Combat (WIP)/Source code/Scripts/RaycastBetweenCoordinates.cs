using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO Maybe replace this script with a boxcast variant when it comes to making detection more sleek
public class RaycastBetweenCoordinates : MonoBehaviour
{
    [Serializable]
    public class OnHitEvent : UnityEvent<RaycastHit2D>
    {
    }

    [Serializable]
    public struct Coordinates
    {
        [SerializeField] internal Transform start;
        [SerializeField] internal Transform end;
    }

    [Serializable]
    public struct RaySettings
    {
        [SerializeField, LabelOverride("Ray Direction")]
        internal Transform rayDir;

        [SerializeField, LabelOverride("Ray Direction Offset In Degrees")]
        internal float rayDirOffset;

        //TODO Replace with LayerMaskSO
        [SerializeField] internal LayerMask layerMask;

        //TODO Replace with IntSO
        [Tooltip("Amount of rays per unity-meter"), LabelOverride("RayCount (per unity-meter)")]
        [SerializeField, Range(2, 999)]
        internal int rayCountPerUnit;

        //TODO Replace with FloatSO
        [SerializeField, Range(0, Mathf.Infinity)]
        internal float rayLength;

        [SerializeField] internal bool interruptOnFirstHit;
    }

    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] private Coordinates _coordinates;
    [SerializeField] private RaySettings _raySettings;
    [SerializeField] private OnHitEvent _onHitEvent = new OnHitEvent();


    void FixedUpdate()
    {
        var length = Vector2.Distance(_coordinates.start.position, _coordinates.end.position);
        var rayCount = (int) (_raySettings.rayCountPerUnit * length);
        var rayPaddingDir = (_coordinates.end.position - _coordinates.start.position).normalized;
        var rayPadding = (length / (rayCount - 1)) * rayPaddingDir;
        var radians = (_raySettings.rayDir.eulerAngles.z) * Mathf.Deg2Rad + _raySettings.rayDirOffset * Mathf.Deg2Rad;
        var dir = new Vector2((float) Math.Cos(radians), (float) Math.Sin(radians));

        for (int i = 0; i < rayCount; i++)
        {
            var o = _coordinates.start.position + rayPadding * i;
            var hit = Physics2D.Raycast(o, dir, _raySettings.rayLength, _raySettings.layerMask);
            if (hit.collider)
            {
                _onHitEvent.Invoke(hit);
                Debug.DrawLine(o, hit.point, Color.cyan);
                if (_raySettings.interruptOnFirstHit)
                    break;
            }
        }
    }
}