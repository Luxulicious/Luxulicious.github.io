using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class PlatformTriggerEnterEvent : UnityEvent<Collider2D> { }
public class PlatformTriggerExitEvent : UnityEvent<Collider2D> { }

/// <summary>
/// Fires an event when entering platform triggers.
/// This makes the logic of the platform independent of whatever this script is attached too.
/// </summary>
public class OnPlatformTrigger : MonoBehaviour
{
    [SerializeField]
    private Collider2D _myCol;
    [HideInInspector]
    public PlatformTriggerExitEvent platformExitEvent = new PlatformTriggerExitEvent();
    [HideInInspector]
    public PlatformTriggerEnterEvent platformEnterEvent = new PlatformTriggerEnterEvent();

    void Start()
    {
        if (_myCol == null)
            _myCol = this.GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<PlatformTriggerArea>() != null)
        {
            platformExitEvent.Invoke(this._myCol);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlatformTriggerArea>() != null)
        {
            platformEnterEvent.Invoke(this._myCol);
        }
    }
}

