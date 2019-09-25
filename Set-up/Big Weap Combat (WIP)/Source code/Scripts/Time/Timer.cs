using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class TimerChangedTextEvent : UnityEvent<string> { }

public class Timer : MonoBehaviour
{
    public bool paused = false;
    [SerializeField]
    private TimerChangedTextEvent _timerChangedTextEvent = new TimerChangedTextEvent();
    [SerializeField, ReadOnly]
    private float _time = 0;


    // Update is called once per frame
    void Update()
    {
        if (paused) return;
        var prevTime = _time;
        _time += Time.deltaTime;
        var timespan = TimeSpan.FromSeconds(_time);
        if (prevTime != _time)
            _timerChangedTextEvent.Invoke(timespan.ToString().Remove(timespan.ToString().Length - 4));
    }

    public void Pause()
    {
        paused = true;
    }
}
