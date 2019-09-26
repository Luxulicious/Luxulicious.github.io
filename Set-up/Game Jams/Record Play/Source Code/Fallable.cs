using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FallEvent : UnityEvent<Fallable> { }

public class Fallable : MonoBehaviour
{
    public GameObject fallableRoot;
    public FallEvent fallEvent = new FallEvent();

    public void Fall()
    {
        fallEvent.Invoke(this);
    }
}
