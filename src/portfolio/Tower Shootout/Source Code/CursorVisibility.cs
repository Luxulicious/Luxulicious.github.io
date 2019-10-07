using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorVisibility : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private bool visible = false;

    void Start()
    {
        Cursor.visible = visible;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Cursor.visible = visible;
    }
}
