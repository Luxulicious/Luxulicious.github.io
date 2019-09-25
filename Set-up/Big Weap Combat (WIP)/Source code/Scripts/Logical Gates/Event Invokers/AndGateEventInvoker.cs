using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AndGateEventInvoker : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] protected BoolEvents _boolEvents = new BoolEvents();
    [SerializeField] protected bool _continuousResult = false;


    public virtual void Update()
    {
        if (_continuousResult)
        {
            _boolEvents.onUpdateEvents.onUpdateTrueEvent.Invoke();
        }
        else
        {
            _boolEvents.onUpdateEvents.onUpdateFalseEvent.Invoke();
        }
        _boolEvents.onUpdateEvents.onUpdateBoolEvent.Invoke(_continuousResult);
    }

    public virtual void FixedUpdate()
    {
        if (_continuousResult)
        {
            _boolEvents.onFixedUpdateEvents.onFixedUpdateTrueEvent.Invoke();
        }
        else
        {
            _boolEvents.onFixedUpdateEvents.onFixedUpdateFalseEvent.Invoke();
        }
        _boolEvents.onFixedUpdateEvents.onFixedUpdateBoolEvent.Invoke(_continuousResult);
    }

    public virtual void Invoke(bool valueA, bool valueB)
    {
        var result = valueA && valueB;
        if (result)
        {
            _boolEvents.invokedEvents.onInvokeTrueEvent.Invoke();
        }
        else
        {
            _boolEvents.invokedEvents.onInvokeFalseEvent.Invoke();
        }
        _boolEvents.invokedEvents.onInvokeBoolEvent.Invoke(result);
        _continuousResult = result;
    }
}