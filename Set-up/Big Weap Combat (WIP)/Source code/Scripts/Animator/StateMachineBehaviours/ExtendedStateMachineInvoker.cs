using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class OnStateEnterEvent : UnityEvent<Animator, AnimatorStateInfo, int> { }

[Serializable]
public class OnStateUpdateEvent : UnityEvent<Animator, AnimatorStateInfo, int> { }

[Serializable]
public class OnStateExitEvent : UnityEvent<Animator, AnimatorStateInfo, int> { }


public interface IExtendedStateMachineListener
{
    void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
}

public class ExtendedStateMachineInvoker : StateMachineBehaviour
{
    [SerializeField, Disable]
    private IExtendedStateMachineListener _listener;
    [SerializeField]
    private OnStateEnterEvent _onStateEnter = new OnStateEnterEvent();
    [SerializeField]
    private OnStateUpdateEvent _onStateUpdate = new OnStateUpdateEvent();
    [SerializeField]
    private OnStateExitEvent _onStateExit = new OnStateExitEvent();


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_listener == null)
            _listener = animator.GetComponent<IExtendedStateMachineListener>();
        _listener.OnStateEnter(animator, stateInfo, layerIndex);

        _onStateEnter.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_listener == null)
            _listener = animator.GetComponent<IExtendedStateMachineListener>();
        _listener.OnStateUpdate(animator, stateInfo, layerIndex);

        _onStateUpdate.Invoke(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_listener == null)
            _listener = animator.GetComponent<IExtendedStateMachineListener>();
        _listener.OnStateExit(animator, stateInfo, layerIndex);

        _onStateExit.Invoke(animator, stateInfo, layerIndex);
    }
}

