using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnScalingUpEvent : UnityEvent<ScaleState>
{
}

[Serializable]
public class OnScalingUpEndedEvent : UnityEvent<ScaleState>
{
}

[Serializable]
public struct ScaleEvents
{
    public OnScalingUpEvent onScalingUpYEvent;
    public OnScalingUpEndedEvent onScalingUpYEndedEvent;

    public void InvokeOnScalingUpYEvent(ScaleState scaleState)
    {
        onScalingUpYEvent.Invoke(scaleState);
    }

    public void InvokeOnScalingUpYEndedEvent(ScaleState scaleState)
    {
        onScalingUpYEndedEvent.Invoke(scaleState);
    }
}