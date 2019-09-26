using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnSpawnedEvent : UnityEvent<Spawnable> { }
[Serializable]
public class OnDespawnedEvent : UnityEvent<Spawnable> { }

public class Spawnable : MonoBehaviour
{
    public static OnSpawnedEvent onSpawnedEvent = new OnSpawnedEvent();
    public static OnDespawnedEvent onDespawnedEvent = new OnDespawnedEvent();

    void OnEnable()
    {
        onSpawnedEvent.Invoke(this);
    }

    void Start()
    {
        onSpawnedEvent.Invoke(this);
    }

    void OnDisable()
    {
        onDespawnedEvent.Invoke(this);
    }

    private void OnDestroy()
    {
        onDespawnedEvent.Invoke(this);
    }
}
