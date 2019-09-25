using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class MousePositionWorldChangedEvent : UnityEvent<Vector2>{}
[Serializable]
public class MousePositionWorldUpdateEvent : UnityEvent<Vector2>{}
[Serializable]
public class MousePositionUpdateEvent : UnityEvent<Vector2>{}
[Serializable]
public class MousePositionChangedEvent : UnityEvent<Vector2>{}

public class MousePosition : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Vector2? _prevMousePosition = null;
    [SerializeField, ReadOnly]
    private Vector2 _mousePosition;

    [Space] [Header("Mouse Position Events")] [SerializeField]
    private MousePositionWorldChangedEvent _mousePositionWorldChangedEvent = new MousePositionWorldChangedEvent();

    [SerializeField]
    private MousePositionWorldUpdateEvent _mousePositionWorldUpdateEvent = new MousePositionWorldUpdateEvent();

    [SerializeField]
    private MousePositionChangedEvent _mousePositionChangedEvent = new MousePositionChangedEvent();
    [SerializeField]
    private MousePositionUpdateEvent _mousePositionUpdateEvent = new MousePositionUpdateEvent();


    void FixedUpdate()
    {
        _mousePosition = Input.mousePosition;
        _mousePositionUpdateEvent.Invoke(_mousePosition);
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(_mousePosition);
        _mousePositionWorldUpdateEvent.Invoke(mousePositionInWorld);
        if (_prevMousePosition.HasValue)
        {
            if (Math.Abs(_prevMousePosition.Value.x - _mousePosition.x) < 0.00000000000001 &&
                Math.Abs(_prevMousePosition.Value.y - _mousePosition.y) < 0.00000000000001) return;
            _mousePositionChangedEvent.Invoke(_mousePosition);
            _mousePositionWorldChangedEvent.Invoke(mousePositionInWorld);
        }
        _prevMousePosition = _mousePosition;
    }
}