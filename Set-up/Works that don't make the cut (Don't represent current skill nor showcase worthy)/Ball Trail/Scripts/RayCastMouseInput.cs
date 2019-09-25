using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class RayCastMouseInput : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private bool _buttonDown = false;
    public LayerMask inputLayer;
    public Clickable latestClickable;

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, inputLayer);
            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject.name);
                var clickable = hit.collider.GetComponent<Clickable>();
                if (clickable)
                {
                    if (Input.GetMouseButtonDown(0))
                        clickable.onClickEnterEvent.Invoke(mousePosition);
                    else
                        clickable.onClickStayEvent.Invoke(mousePosition);
                    if (latestClickable)
                    {
                        if (latestClickable != clickable)
                        {
                            latestClickable.onClickExitEvent.Invoke(mousePosition);
                            latestClickable = clickable;
                        }
                    }
                    else
                    {
                        latestClickable = clickable;
                    }
                }
            }
            else if (latestClickable)
            {
                latestClickable.onClickExitEvent.Invoke(mousePosition);
                latestClickable = null;
            }
            _buttonDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_buttonDown)
            {
                if (latestClickable)
                    latestClickable.onClickExitEvent.Invoke(mousePosition);
            }
            _buttonDown = false;
        }

    }

}
