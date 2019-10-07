using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DeathEvent : UnityEvent<GameObject> { }

public class OnTriggerExitDeath : MonoBehaviour
{

    public DeathEvent deathEvent;

    void OnTriggerExit2D(Collider2D col)
    {
        deathEvent.Invoke(col.gameObject);
    }
}
