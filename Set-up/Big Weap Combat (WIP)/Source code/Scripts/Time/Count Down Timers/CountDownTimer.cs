using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;


[Serializable]
public class FloatBetweenValuesEvent : UnityEvent<float> { }

[Serializable]
public class FloatLargerThanEvent : UnityEvent<float> { }

[Serializable]
public class FloatSmallerThanEvent : UnityEvent<float> { }

[Serializable]
public class FloatSmallerThanEventEmitter
{
    public float threshold;

    [SerializeField]
    private FloatSmallerThanEvent _floatSmallerThanEvent = new FloatSmallerThanEvent();

    public void TryInvoke(float f)
    {
        if (f < threshold)
            _floatSmallerThanEvent.Invoke(f);
    }
}

[Serializable]
public class FloatLargerThanEventEmitter
{
    public float threshold;

    [SerializeField]
    private FloatLargerThanEvent _floatLargerThanEvent = new FloatLargerThanEvent();

    public void TryInvoke(float f)
    {
        if (f > threshold)
            _floatLargerThanEvent.Invoke(f);
    }
}

[Serializable]
public class FloatBetweenValuesEventEmitter
{
    public float min, max;

    [SerializeField]
    private FloatBetweenValuesEvent _floatBetweenValuesEvent = new FloatBetweenValuesEvent();

    public bool IsBetweenValues(float f)
    {
        if (max < min)
        {
            Debug.LogError("Min > Max");
            return false;
        }
        else
            return min < f && max > f;
    }

    public void TryInvoke(float f)
    {
        if (IsBetweenValues(f))
            _floatBetweenValuesEvent.Invoke(f);
    }
}

[Serializable]
public class TimerUpdateEvent : UnityEvent<float> { }

[Serializable]
public class TimerEndedEvent : UnityEvent { }

public class CountDownTimer : MonoBehaviour
{
    public bool startOnAwake = true;
    [SerializeField]
    private float _startTime = 10f;
    [SerializeField]
    private float _time = 10f;
    [SerializeField]
    private float _floatComparisonThreshold = 1e-37f;

    [Space, Header("Events")]
    [SerializeField]
    private TimerUpdateEvent _timerUpdateEvent = new TimerUpdateEvent();
    [SerializeField]
    public TimerEndedEvent timerEndedEvent = new TimerEndedEvent();
    [SerializeField]
    private List<FloatBetweenValuesEventEmitter> _timeBetweenValuesEvents = new List<FloatBetweenValuesEventEmitter>();
    [SerializeField]
    private List<FloatLargerThanEventEmitter> _timeLargerThanEventEmitter = new List<FloatLargerThanEventEmitter>();
    [SerializeField]
    private List<FloatSmallerThanEventEmitter> _timeSmallerThanEventEmitter = new List<FloatSmallerThanEventEmitter>();

    public void Awake()
    {
        if(_startTime <= 0)
        _startTime = _time;
        if (startOnAwake)
            StartTimer();
    }

    public void StartTimer()
    {
        StopCoroutine(Tick());
        StartCoroutine(Tick());
    }

    public void StartTimer(Collision2D col)
    {
        StartTimer();
    }

    public void ResetTimer()
    {
        _time = _startTime;
        StopTimer();
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        //StopCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (_time > 0)
        {
            var prevTime = _time;
            _time -= Time.deltaTime;

            if (_time <= 0)
            {
                _time = 0;
                timerEndedEvent.Invoke();
            }

            if (!FloatExtensions.Equals(_time, prevTime, _floatComparisonThreshold))
                _timerUpdateEvent.Invoke(_time);

            //TODO Optimize by making it an ordered list
            _timeBetweenValuesEvents.ForEach(x => x.TryInvoke(_time));
            _timeLargerThanEventEmitter.ForEach(x => x.TryInvoke(_time));
            _timeSmallerThanEventEmitter.ForEach(x => x.TryInvoke(_time));

            yield return new WaitForEndOfFrame();
        }
        timerEndedEvent.Invoke();
    }

    public void SetTime(float time)
    {
        this._time = time;
    }

    public float GetTime()
    {
        return _time;
    }

    public void SetStartTime(float startTime)
    {
        this._startTime = startTime;
    }
}