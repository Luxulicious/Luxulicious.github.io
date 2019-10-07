using System;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using Util;
using Time = UnityEngine.Time;

/// <summary>
/// Thrown when time left has changed
/// Param: Time left
/// </summary>
[Serializable]
public class TimeLeftEvent : UnityEvent<float> { }

/// <summary>
/// Thrown when time has run out
/// Param: Time left
/// </summary>
[Serializable]
public class TimeExpiredEvent : UnityEvent<float> { }

/// <summary>
/// Thrown when time is about to run out
/// </summary>
[Serializable]
public class TimeLowEvent : UnityEvent { }

/// <summary>
/// Count down timer; Uses photon time
/// </summary>
public class CountDownTimer : MonoBehaviourPun
{
    public TimeLeftEvent timeLeftEvent = new TimeLeftEvent();
    public TimeExpiredEvent timeExpiredEvent = new TimeExpiredEvent();
    public TimeLowEvent timeLowEvent = new TimeLowEvent();

    public float maxTimeElapsed = 30f;
    public float lowTimeThreshold = 11f;
    [SerializeField, ReadOnly] private float _timeElapsed;
    [SerializeField, ReadOnly] private float? _startTime;
    private bool _textIsPulsing = false;

    /// <summary>
    /// Starts the timer
    /// </summary>
    public void StartTimer()
    {
        _startTime = (float)PhotonNetwork.Time;
    }

    void Update()
    {
        if (_startTime.HasValue)
        {
            _timeElapsed = (float)PhotonNetwork.Time - _startTime.Value;
            if (!_textIsPulsing && maxTimeElapsed - _timeElapsed <= lowTimeThreshold)
            {
                _textIsPulsing = true;
                timeLowEvent.Invoke();
            }
            if (maxTimeElapsed > _timeElapsed)
            {
                timeLeftEvent.Invoke(maxTimeElapsed - _timeElapsed);
            }
            else
            {
                timeExpiredEvent.Invoke(0);
            }
        }
    }

    public void RequestStartTime(Player player)
    {
        if(!photonView.IsMine && !PhotonNetwork.IsMasterClient)
            photonView.RPC("RequestStartTimeRPC", RpcTarget.MasterClient, player);
    }

    [PunRPC]
    private void RequestStartTimeRPC(Photon.Realtime.Player player)
    {
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GiveStartTimeRPC", player, _startTime);
        }
    }

    [PunRPC]
    private void GiveStartTimeRPC(float time)
    {
        Debug.Log(TimeSpan.FromSeconds(time).ToString().Remove(TimeSpan.FromSeconds(time).ToString().Length - 4));
        _startTime = time;
    }

}