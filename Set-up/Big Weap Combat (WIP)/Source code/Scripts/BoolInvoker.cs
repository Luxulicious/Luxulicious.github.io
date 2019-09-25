using System.Collections;
using System.Collections.Generic;
using Assets.Experimental.ScriptableObjects.Scripts.Events.Bool;
using UnityEngine;

public class BoolInvoker : MonoBehaviour
{
    [SerializeField]
    BoolEventSO _event;

    void Awake()
    {
        _event.Invoke(true);
    }
}
