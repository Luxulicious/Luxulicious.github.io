using System;
using System.Collections;
using System.Collections.Generic;
using ByteSheep.Events;
using UnityEngine;

[Serializable]
public class OnScoreUpdateEvent : QuickEvent<int> {}

public class Score : MonoBehaviour
{
    [LabelOverride("Score"), SerializeField, Observe("InvokeOnScoreUpdateEvent")]
    private int _value = 0;

    [SerializeField]
    private OnScoreUpdateEvent _onScoreUpdateEvent = new OnScoreUpdateEvent();

    public void InvokeOnScoreUpdateEvent()
    {
        _onScoreUpdateEvent.Invoke(_value);
    }

    public void IncrementScore(int increment)
    {
        _value += increment;
        InvokeOnScoreUpdateEvent();
    }

    public void DecrementScore(int decrement)
    {
        _value -= decrement;
        InvokeOnScoreUpdateEvent();
    }
}
