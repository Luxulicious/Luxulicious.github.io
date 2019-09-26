using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Util;

/// <summary>
/// Thrown when there are no catch attempts left
/// </summary>
[Serializable]
public class OnNoAttemptsLeft : UnityEvent
{
}

/// <summary>
/// Thrown when catch attempts left changed to update all observers of the amount left
/// </summary>
[Serializable]
public class AttemptsLeftEvent : UnityEvent<int>
{
}

/// <summary>
/// Thrown when a catch attempt failed/missed
/// </summary>
[Serializable]
public class OnAttemptFailedEvent : UnityEvent<int>
{
}

/// <summary>
/// Thrown when an catch attempt succeeded/hit
/// </summary>
[Serializable]
public class OnAttemptSucceededEvent : UnityEvent<int>
{
}

/// <summary>
/// Keeps track of the amount times a catch attempt connected, missed and how many are left
/// </summary>
public class Attempts : MonoBehaviourPun
{
    public static Attempts instance;
    public AttemptsLeftEvent attemptsLeftEvent = new AttemptsLeftEvent();
    public OnAttemptFailedEvent onAttemptFailedEvent = new OnAttemptFailedEvent();
    public OnAttemptSucceededEvent onAttemptSucceededEvent = new OnAttemptSucceededEvent();
    public OnNoAttemptsLeft onNoAttemptsLeft = new OnNoAttemptsLeft();

    [SerializeField, Range(0, 100)] private int _attemptsLeft = 10;
    [SerializeField, ReadOnly] private int _attemptsFailed = 0;
    [SerializeField, ReadOnly] private int _attemptsSucceeded = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        attemptsLeftEvent.Invoke(_attemptsLeft);
    }

    public void Attempt(bool success)
    {
        if (!photonView.IsMine) return;
        if (_attemptsLeft > 0)
            _attemptsLeft--;
        else
        {
            onNoAttemptsLeft.Invoke();
            return;
        }

        attemptsLeftEvent.Invoke(_attemptsLeft);

        if (success)
        {
            _attemptsSucceeded++;
            onAttemptSucceededEvent.Invoke(_attemptsSucceeded);
        }
        else
        {
            _attemptsFailed++;
            onAttemptFailedEvent.Invoke(_attemptsFailed);
        }
    }
}