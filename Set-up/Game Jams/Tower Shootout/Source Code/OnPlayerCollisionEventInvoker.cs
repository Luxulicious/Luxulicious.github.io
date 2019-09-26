using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class OnPlayerCollisionEnterEvent : UnityEvent<Collision2D> { }
[Serializable]
public class OnPlayerCollisionStayEvent : UnityEvent<Collision2D> { }
[Serializable]
public class OnPlayerCollisionExitEvent : UnityEvent<Collision2D> { }

[RequireComponent(typeof(Collider2D))]
public class OnPlayerCollisionEventInvoker : MonoBehaviour
{
    public OnCollisionEnter2DEvent _onPlayerCollisionEnterEvent = new OnCollisionEnter2DEvent();
    public OnCollisionStay2DEvent _onPlayerCollisionStayEvent = new OnCollisionStay2DEvent();
    public OnCollisionExit2DEvent _onPlayerCollisionExitEvent = new OnCollisionExit2DEvent();

    private void OnCollisionEnter2D(Collision2D col)
    {
        _onPlayerCollisionEnterEvent.Invoke(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        _onPlayerCollisionExitEvent.Invoke(col);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        _onPlayerCollisionStayEvent.Invoke(col);
    }

    //TODO Move this elsewhere
    private static string playerTag = "Player";
    public bool IsPlayerCollision(Collision2D col)
    {
        return col.collider.tag == playerTag || col.otherCollider.tag == playerTag;
    }
}

