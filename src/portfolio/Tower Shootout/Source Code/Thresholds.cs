using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class WithinThresholdsChangedEvent : UnityEvent<bool> { }

public class Thresholds : MonoBehaviour
{
    public WithinThresholdsChangedEvent withinThresholdsEvent = new WithinThresholdsChangedEvent();

    public float min;
    public float max;
    [SerializeField]
    private bool _nextFixedUpdateInvokeDefault = true;
    [SerializeField]
    private bool _defaultValue = false;

    public void WithinThresholds(float val)
    {
        withinThresholdsEvent.Invoke(val >= min && val <= max);
        if (_nextFixedUpdateInvokeDefault)
            StartCoroutine(NextFixedUpdateResetToDefault());
    }

    private IEnumerator NextFixedUpdateResetToDefault()
    {
        yield return new WaitForFixedUpdate();
        withinThresholdsEvent.Invoke(_defaultValue);
    }
}



//[SerializeField, ReadOnly]
//private float _val;

//public void InThresholds(float val)
//{
//    if (_val != val)
//        withinThresholdsChangedEvent.Invoke(min <= val && max >= val);
//    _val = val;
//}

