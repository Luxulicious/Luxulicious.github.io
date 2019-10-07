using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class KeyClickEvent : UnityEvent<float> { }
[Serializable]
public class KeyDownEvent : UnityEvent { }
[Serializable]
public class KeyUpEvent : UnityEvent<float> { }

public class KeyInput : MonoBehaviour
{
    [SerializeField]
    private string _inputName;
    [SerializeField]
    private KeyCode _keyCode;
    [SerializeField]
    private KeyClickEvent _keyClickEvent = new KeyClickEvent();
    [SerializeField]
    private KeyDownEvent _keyDownEvent = new KeyDownEvent();
    [SerializeField]
    private KeyUpEvent _keyUpEvent = new KeyUpEvent();

    private float _keyboardClickTime = 0;

    void Start()
    {
        if (String.IsNullOrEmpty(_inputName))
            this._inputName = this.name;
        else
            this.name = this._inputName;
    }

    void Update()
    {
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
