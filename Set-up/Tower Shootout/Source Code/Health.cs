using System;
using System.Collections;
using System.Collections.Generic;
using ByteSheep.Events;
using UnityEngine;

[Serializable]
public class OnDeathEvent : QuickEvent { }

public class Health : MonoBehaviour
{
    [SerializeField]
    private OnDeathEvent _onDeathEvent = new OnDeathEvent();
    [SerializeField]
    private double _defaultHealth = 1;
    [SerializeField]
    private double _health = 1;

    void Awake()
    {
        if (_health <= 0)
            _health = _defaultHealth;
    }

    public void Damage(double damage)
    {
        if (_health <= 0) return;
        _health -= damage;
        if (_health > 0) return;
        _health = 0;
        _onDeathEvent.Invoke();
    }

    public void ResetHealth()
    {
        _health = _defaultHealth;
    }
}
