using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class DestroyDestructableEvent : UnityEvent<Destructable> { }

public class Destructable : MonoBehaviour
{
    public DestroyDestructableEvent destroyDestructableEvent = new DestroyDestructableEvent();
    public GameObject destructableRoot;

    public void Destroy()
    {
        destroyDestructableEvent.Invoke(this);
    }
}

