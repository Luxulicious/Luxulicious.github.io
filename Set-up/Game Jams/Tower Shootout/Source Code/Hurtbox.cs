using System;
using System.Collections;
using System.Collections.Generic;
using ByteSheep.Events;
using UnityEngine;

[Serializable]
public class OnHitEvent : QuickEvent { }

[Serializable]
public class OnDamageEvent : QuickEvent<double> { }

public class Hurtbox : MonoBehaviour
{
    [SerializeField]
    private OnHitEvent _onHitEvent = new OnHitEvent();
    [SerializeField]
    private OnDamageEvent _onDamageEvent = new OnDamageEvent();

    public void Hit(double damage)
    {
        _onHitEvent.Invoke();
        _onDamageEvent.Invoke(damage);
    }
}