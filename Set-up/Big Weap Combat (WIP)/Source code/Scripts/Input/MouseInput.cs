using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Events.MouseInputEvents;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fires events based on mouse input.
/// </summary>
public class MouseInput : MonoBehaviour
{
    private Vector2? _prevMousePosition = null;
    private Vector2 _mousePosition;

    [Header("Controls")]
    [SerializeField]
    private KeyCode _primary = KeyCode.Mouse0;
    [SerializeField]
    private KeyCode _secondary = KeyCode.Mouse1;
    [SerializeField]
    private KeyCode _tertiary = KeyCode.Mouse2;

    [Space]
    [Header("Mouse Position Events")]
    [SerializeField]
    private MousePositionUpdateEvent _mousePositionUpdateEvent = new MousePositionUpdateEvent();
    [SerializeField]
    private ContinuousMousePositionEvent _continuousMousePositionEvent = new ContinuousMousePositionEvent();

    [Space]
    [Header("Mouse Button Events")]
    [SerializeField]
    private MouseButtonPrimaryEvent _mouseButtonPrimaryEvent = new MouseButtonPrimaryEvent();
    [SerializeField]
    private MouseButtonSecondaryClickEvent _mouseButtonSecondaryEvent = new MouseButtonSecondaryClickEvent();
    [SerializeField]
    private MouseButtonTertiaryEvent _mouseButtonTertiaryEvent = new MouseButtonTertiaryEvent();

    [Space]
    [Header("Mouse Button Up Events")]
    [SerializeField]
    private MouseButtonPrimaryUpEvent _mouseButtonPrimaryUpEvent = new MouseButtonPrimaryUpEvent();
    [SerializeField]
    private MouseButtonSecondaryUpEvent _mouseButtonSecondaryUpEvent = new MouseButtonSecondaryUpEvent();
    [SerializeField]
    private MouseButtonTertiaryUpEvent _mouseButtonTertiaryUpEvent = new MouseButtonTertiaryUpEvent();

    [Space]
    [Header("Mouse Button Down Events")]
    [SerializeField]
    private MouseButtonPrimaryDownEvent _mouseButtonPrimaryDownEvent = new MouseButtonPrimaryDownEvent();
    [SerializeField]
    private MouseButtonSecondaryDownEvent _mouseButtonSecondaryDownEvent = new MouseButtonSecondaryDownEvent();
    [SerializeField]
    private MouseButtonTertiaryDownEvent _mouseButtonTertiaryDownEvent = new MouseButtonTertiaryDownEvent();


    private float _mouseButtonPrimaryTime = 0;
    private float _mouseButtonSecondaryTime = 0;
    private float _mouseButtonTertiaryTime = 0;

    void Start()
    {
        if (_primary != KeyCode.Mouse0 && _primary != KeyCode.Mouse1 && _primary != KeyCode.Mouse2 && _primary != KeyCode.Mouse3 && _primary != KeyCode.Mouse4 && _primary != KeyCode.Mouse5 && _primary != KeyCode.Mouse6)
            _primary = KeyCode.Mouse0;
        if (_secondary != KeyCode.Mouse1 && _secondary != KeyCode.Mouse1 && _secondary != KeyCode.Mouse2 && _secondary != KeyCode.Mouse3 && _secondary != KeyCode.Mouse4 && _secondary != KeyCode.Mouse5 && _secondary != KeyCode.Mouse6)
            _secondary = KeyCode.Mouse1;
        if (_tertiary != KeyCode.Mouse2 && _tertiary != KeyCode.Mouse1 && _tertiary != KeyCode.Mouse2 && _tertiary != KeyCode.Mouse3 && _tertiary != KeyCode.Mouse4 && _tertiary != KeyCode.Mouse5 && _tertiary != KeyCode.Mouse6)
            _tertiary = KeyCode.Mouse2;
        if (_primary == _secondary || _primary == _tertiary || _secondary == _tertiary)
        {
            _primary = KeyCode.Mouse0;
            _secondary = KeyCode.Mouse1;
            _tertiary = KeyCode.Mouse2;
        }
    }

    void FixedUpdate()
    {
        CheckMousePosition();
        CheckPrimaryClick();
        CheckSecondaryClick();
        CheckTertiaryClick();
    }

    private void CheckMousePosition()
    {
        _mousePosition = Input.mousePosition;
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(_mousePosition);
        _continuousMousePositionEvent.Invoke(mousePositionInWorld);
        if (_prevMousePosition.HasValue)
        {
            if (Math.Abs(_prevMousePosition.Value.x - _mousePosition.x) < 0.1 && Math.Abs(_prevMousePosition.Value.y - _mousePosition.y) < 0.1) return;
            _mousePositionUpdateEvent.Invoke(mousePositionInWorld);
        }
        _prevMousePosition = _mousePosition;
    }

    private void CheckPrimaryClick()
    {
        if (Input.GetKey(_primary))
        {
            if (_mouseButtonPrimaryTime <= 0)
                _mouseButtonPrimaryDownEvent.Invoke();
            _mouseButtonPrimaryTime += Time.deltaTime;
            _mouseButtonPrimaryEvent.Invoke(_mouseButtonPrimaryTime);
        }
        else
        {
            if (_mouseButtonPrimaryTime > 0)
                _mouseButtonPrimaryUpEvent.Invoke(_mouseButtonPrimaryTime);
            _mouseButtonPrimaryTime = 0;
        }
    }

    private void CheckSecondaryClick()
    {
        if (Input.GetKey(_secondary))
        {
            if (_mouseButtonSecondaryTime <= 0)
                _mouseButtonSecondaryDownEvent.Invoke();
            _mouseButtonSecondaryTime += Time.deltaTime;
            _mouseButtonSecondaryEvent.Invoke(_mouseButtonSecondaryTime);
        }
        else
        {
            if (_mouseButtonSecondaryTime > 0)
                _mouseButtonSecondaryUpEvent.Invoke(_mouseButtonSecondaryTime);
            _mouseButtonSecondaryTime = 0;
        }
    }

    private void CheckTertiaryClick()
    {
        if (Input.GetKey(_tertiary))
        {
            if (_mouseButtonTertiaryTime <= 0)
                _mouseButtonTertiaryDownEvent.Invoke();
            _mouseButtonTertiaryTime += Time.deltaTime;
            _mouseButtonTertiaryEvent.Invoke(_mouseButtonTertiaryTime);
        }
        else
        {
            if (_mouseButtonTertiaryTime > 0)
                _mouseButtonTertiaryUpEvent.Invoke(_mouseButtonTertiaryTime);
            _mouseButtonTertiaryTime = 0;
        }
    }

    public Vector2 GetMousePosition()
    {
        return _mousePosition;
    }
}
