using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnCollisionEnter2DEvent : UnityEvent<Collision2D> { }
[Serializable]
public class OnCollisionStay2DEvent : UnityEvent<Collision2D> { }
[Serializable]
public class OnCollisionExit2DEvent : UnityEvent<Collision2D> { }

public class OnCollisionEventInvoker : MonoBehaviour
{
    public OnCollisionEnter2DEvent onCollisionEnter2DEvent = new OnCollisionEnter2DEvent();
    public OnCollisionStay2DEvent onCollisionStay2DEvent = new OnCollisionStay2DEvent();
    public OnCollisionExit2DEvent onCollisionExit2DEvent = new OnCollisionExit2DEvent();

    private void OnCollisionEnter2D(Collision2D col)
    {
        onCollisionEnter2DEvent.Invoke(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        onCollisionExit2DEvent.Invoke(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        onCollisionStay2DEvent.Invoke(col);
    }
}
