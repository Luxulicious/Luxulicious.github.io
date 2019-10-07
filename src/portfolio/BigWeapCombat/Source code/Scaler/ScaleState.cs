using System;
using UnityEngine;

[Serializable]
public struct ScaleState
{
    //TODO Replace timeScalingUpX/Y with Vector2
    //TODO Replace scalingUpX/Y with Bool2(?)
    [Header("X-state")]
    [Disable] public bool scalingUpX;
    [Disable] public float timeScalingUpX;
    [Disable] public bool scalingDownX;
    [Disable] public float timeScalingDownX;
    [Header("Y-state")]
    [Disable] public bool scalingUpY;
    [Disable] public float timeScalingUpY;
    [Disable] public bool scalingDownY;
    [Disable] public float timeScalingDownY;
    [Header("Full-state")]
    [Disable] public Vector3 prevScale;
    [Disable] public Vector3 currentScale;
}