using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StateCallback
{
    public string stateName;
    public OnStateEnterEvent onStateEnterEvent = new OnStateEnterEvent();
    public OnStateUpdateEvent onStateUpdateEvent = new OnStateUpdateEvent();
    public OnStateExitEvent onStateExitEvent = new OnStateExitEvent();
}

[ExecuteAlways]
[RequireComponent(typeof(Animator))]
public class ExtendedStateMachineListener : MonoBehaviour, IExtendedStateMachineListener
{
    private static string _unknown = "???????????????????";
    [SerializeField, Disable]
    private string _currentStateName;
    [SerializeField]
    private List<StateCallback> _stateCallbacks = new List<StateCallback>();
    [SerializeField, Disable]
    private StateCallback _currentStateCallback;

    void Update()
    {
        CheckContainsIllegalCallback();
    }

    void CheckContainsIllegalCallback()
    {
        var dupes = _stateCallbacks.Where(item => item.stateName != null)
                                   .GroupBy(item => item.stateName)
                                   .Any(g => g.Count() > 1);

        if (dupes)
            Debug.LogWarning("No duplicates allowed in callbacks, please remove any callbacks with a duplicate name!");
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //TODO Optimize this comparison...
        _currentStateCallback = _stateCallbacks.FirstOrDefault(x => stateInfo.IsName(x.stateName));
        if (_currentStateCallback != null)
        {
            _currentStateName = _currentStateCallback.stateName;
            _currentStateCallback.onStateEnterEvent.Invoke(animator, stateInfo, layerIndex);
        }
        else
        {
            _currentStateName = _unknown;
            Debug.LogWarning("Unknown state was attempted to invoke: " + stateInfo);
        }
    }

    public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_currentStateCallback != null)
        {
            _currentStateCallback.onStateUpdateEvent.Invoke(animator, stateInfo, layerIndex);
            if(!string.IsNullOrEmpty(_currentStateName))
                _currentStateName = _currentStateCallback.stateName;
        }
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_currentStateCallback != null)
            _currentStateCallback.onStateExitEvent.Invoke(animator, stateInfo, layerIndex);
        _currentStateName = _unknown;
    }
}