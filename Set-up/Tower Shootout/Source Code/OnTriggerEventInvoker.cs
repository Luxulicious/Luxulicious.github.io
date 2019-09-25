using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnTriggerEnter2DEvent : UnityEvent<Collider2D> { }
[Serializable]
public class OnTriggerStay2DEvent : UnityEvent<Collider2D> { }
[Serializable]
public class OnTriggerExit2DEvent : UnityEvent<Collider2D> { }

public class OnTriggerEventInvoker : MonoBehaviour
{
    public OnTriggerEnter2DEvent onTriggerEnter2DEvent = new OnTriggerEnter2DEvent();
    public OnTriggerStay2DEvent onTriggerStay2DEvent = new OnTriggerStay2DEvent();
    public OnTriggerExit2DEvent onTriggerExit2DEvent = new OnTriggerExit2DEvent();

    private void OnTriggerEnter2D(Collider2D col)
    {
        onTriggerEnter2DEvent.Invoke(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        onTriggerExit2DEvent.Invoke(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        onTriggerStay2DEvent.Invoke(col);
    }
}
