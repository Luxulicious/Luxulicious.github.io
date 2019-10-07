using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Re-invokes Input.GetKey()
/// </summary>
[Serializable]
public class KeyClickEvent : UnityEvent<float> { }
/// <summary>
/// Re-invokes Input.GetKeyDown()
/// </summary>
[Serializable]
public class KeyDownEvent : UnityEvent { }
/// <summary>
/// Re-invokes Input.GetKeyUp()
/// </summary>
[Serializable]
public class KeyUpEvent : UnityEvent<float> { }

/// <summary>
/// Re-invokes keyboard event, so that they are visible in the inspector.
/// </summary>
public class KeyInput : MonoBehaviourPun
{
    [SerializeField]
    private KeyCode _keyCode;
    [SerializeField]
    private KeyClickEvent _keyClickEvent = new KeyClickEvent();
    [SerializeField]
    private KeyDownEvent _keyDownEvent = new KeyDownEvent();
    [SerializeField]
    private KeyUpEvent _keyUpEvent = new KeyUpEvent();

    private float _keyboardClickTime = 0;



    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKey(_keyCode))
        {
            if (_keyboardClickTime <= 0)
                _keyDownEvent.Invoke();
            _keyboardClickTime += Time.deltaTime;
            _keyClickEvent.Invoke(_keyboardClickTime);
        }
        else
        {
            if (_keyboardClickTime > 0)
                _keyUpEvent.Invoke(_keyboardClickTime);
            _keyboardClickTime = 0;
        }
    }
}
