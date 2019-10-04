using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listener for Attempts and re-invokes the events here.
/// Makes attempts scene undependable.
/// </summary>
public class AttemptsListener : MonoBehaviour
{
    public AttemptsLeftEvent attemptsLeftEvent = new AttemptsLeftEvent();
    public OnAttemptFailedEvent onAttemptFailedEvent = new OnAttemptFailedEvent();
    public OnAttemptSucceededEvent onAttemptSucceededEvent = new OnAttemptSucceededEvent();
    public OnNoAttemptsLeft onNoAttemptsLeft = new OnNoAttemptsLeft();

    void Start()
    {
        Attempts.instance.attemptsLeftEvent.AddListener(InvokeOnAttemptEvent);
        Attempts.instance.onAttemptFailedEvent.AddListener(InvokeOnAttemptFailedEvent);
        Attempts.instance.onAttemptSucceededEvent.AddListener(InvokeOnAttemptSucceededEvent);
        Attempts.instance.onNoAttemptsLeft.AddListener(InvokeOnNoAttemptsLeft);
    }

    private void InvokeOnAttemptEvent(int count)
    {
        attemptsLeftEvent.Invoke(count);
    }

    private void InvokeOnAttemptFailedEvent(int count)
    {
        onAttemptFailedEvent.Invoke(count);
    }

    private void InvokeOnAttemptSucceededEvent(int count)
    {
        onAttemptSucceededEvent.Invoke(count);
    }

    private void InvokeOnNoAttemptsLeft()
    {
        onNoAttemptsLeft.Invoke();
    }
}