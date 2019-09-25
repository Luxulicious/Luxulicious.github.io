using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnDeathEvent : UnityEvent { }

public class Health : MonoBehaviour
{
    public float health = 1;
    [SerializeField]
    private OnDeathEvent _onDeathEvent = new OnDeathEvent();

    public void Damage(float damage)
    {
        health -= Mathf.Clamp((float)damage, 0f, (float)health);
        if (health <= 0)
            _onDeathEvent.Invoke();
    }
}
