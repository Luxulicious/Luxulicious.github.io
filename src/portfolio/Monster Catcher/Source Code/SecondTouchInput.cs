using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Util;

/// <summary>
/// Reflects touch enter
/// </summary>
[Serializable]
public class OnSecondTouchEnterEvent : UnityEvent<Vector2>
{
}

/// <summary>
/// Reflects touch exit
/// </summary>
[Serializable]
public class OnSecondTouchExitEvent : UnityEvent<Vector2, float>
{
}

/// <summary>
/// Reflects touch stay
/// </summary>
[Serializable]
public class OnSecondTouchStayEvent : UnityEvent<Vector2, float>
{
}

/// <summary>
/// Invokes events on second touch, so that they are visible in the inspector.
/// </summary>
public class SecondTouchInput : MonoBehaviourPun
{
    public OnSecondTouchEnterEvent onSecondTouchEnterEvent = new OnSecondTouchEnterEvent();
    public OnSecondTouchExitEvent onSecondTouchExitEvent = new OnSecondTouchExitEvent();
    public OnSecondTouchStayEvent onSecondTouchStayEvent = new OnSecondTouchStayEvent();
    [SerializeField, ReadOnly]
    private float _elapsedTimeSinceTouch = 0;

    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.touchCount >= 2)
        {
            var secondTouch = Input.touches[1];
            var secondTouchPhase = secondTouch.phase;
            if (secondTouchPhase == TouchPhase.Began)
            {
                onSecondTouchEnterEvent.Invoke(secondTouch.position);
                _elapsedTimeSinceTouch += Time.deltaTime;
            }
            else if (secondTouchPhase == TouchPhase.Moved || secondTouchPhase == TouchPhase.Stationary)
            {
                _elapsedTimeSinceTouch += Time.deltaTime;
                onSecondTouchStayEvent.Invoke(secondTouch.position, _elapsedTimeSinceTouch);
            }
            else if (secondTouchPhase == TouchPhase.Ended || secondTouchPhase == TouchPhase.Canceled)
            {
                onSecondTouchExitEvent.Invoke(secondTouch.position, _elapsedTimeSinceTouch);
                _elapsedTimeSinceTouch = 0;
            }
        } 
    }
}