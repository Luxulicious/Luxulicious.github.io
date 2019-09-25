using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class OnDropEnterEvent : UnityEvent<Dropper>
{
}

[Serializable]
public class OnDropExitEvent : UnityEvent<Dropper>
{
}

[Serializable]
public class OnDropStayEvent : UnityEvent<Dropper>
{
}

[Serializable]
public class OnRemoveLiveDropperEvent : UnityEvent<Dropper>
{
}

[Serializable]
public class OnAddedLiveDropperEvent : UnityEvent<Dropper>
{
}

/// <summary>
/// Attach to things that can drop through dropthroughables
/// </summary>
public class Dropper : MonoBehaviour
{
    [SerializeField]
    private List<Collider2D> _colliders;

    public List<Collider2D> Colliders
    {
        get { return _colliders; }
        set { _colliders = value; }
    }

    #region Live Droppers & - Events

    private static List<Dropper> _liveDroppers = new List<Dropper>();
    public static OnRemoveLiveDropperEvent onRemoveLiveDropperEvent = new OnRemoveLiveDropperEvent();
    public static OnAddedLiveDropperEvent onAddedLiveDropperEvent = new OnAddedLiveDropperEvent();

    #endregion

    #region Drop Events

    private OnDropEnterEvent _onDropEnterEvent = new OnDropEnterEvent();
    private OnDropExitEvent _onDropExitEvent = new OnDropExitEvent();
    private OnDropStayEvent _onDropStayEvent = new OnDropStayEvent();
    [SerializeField, ReadOnly]
    private bool _onDropEventActive;

    #endregion

    void OnEnable()
    {
        if (!_colliders.Any())
            Debug.LogError("Requires a Collider2D component.", this);
        AddLiveDropper(this);
    }

    void OnDisable()
    {
        RemoveLiveDropper(this);
    }

    #region Live Droppers Accessors

    public static void AddLiveDropper(Dropper dropper)
    {
        if (!_liveDroppers.Contains(dropper) ||
            _liveDroppers.All(x => x.gameObject != dropper.gameObject))
        {
            _liveDroppers.Add(dropper);
            onAddedLiveDropperEvent.Invoke(dropper);
        }
    }

    public static bool RemoveLiveDropper(Dropper dropper)
    {
        var success = _liveDroppers.Remove(dropper);
        if (success)
            onRemoveLiveDropperEvent.Invoke(dropper);
        return success;
    }

    public static List<Dropper> GetLiveDroppers()
    {
        return _liveDroppers;
    }

    #endregion

    #region Drop Event Invocation

    public bool OnDropEventActive
    {
        get { return _onDropEventActive; }
    }

    public void InvokeOnDropEnterEvent()
    {
        if (!_onDropEventActive)
        {
            _onDropEnterEvent.Invoke(this);
            _onDropEventActive = true;
        }
    }

    public void InvokeOnDropExitEvent()
    {
        if (_onDropEventActive)
        {
            _onDropExitEvent.Invoke(this);
            _onDropEventActive = false;
        }
    }

    public void InvokeOnDropStayEvent()
    {
        if (_onDropEventActive)
        {
            _onDropStayEvent.Invoke(this);
        }
    }

    #endregion

    #region Drop Event Listener Access

    public void AddListenerOnDropEnterEvent(UnityAction<Dropper> listener)
    {
        _onDropEnterEvent.AddListener(listener);
    }

    public void AddListenerOnDropExitEvent(UnityAction<Dropper> listener)
    {
        _onDropExitEvent.AddListener(listener);
    }

    public void AddListenerOnDropStayEvent(UnityAction<Dropper> listener)
    {
        _onDropStayEvent.AddListener(listener);
    }

    public void RemoveListenerOnDropEnterEvent(UnityAction<Dropper> listener)
    {
        _onDropEnterEvent.RemoveListener(listener);
    }

    public void RemoveListenerOnDropExitEvent(UnityAction<Dropper> listener)
    {
        _onDropExitEvent.RemoveListener(listener);
    }

    public void RemoveListenerOnDropStayEvent(UnityAction<Dropper> listener)
    {
        _onDropStayEvent.RemoveListener(listener);
    }

    #endregion
}