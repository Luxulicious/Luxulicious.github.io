using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class Dragable : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Vector2 _startPos;
    //Position previous update call
    [SerializeField, ReadOnly]
    private Vector2 _prevPos;
    //Position current update call
    [SerializeField, ReadOnly]
    private Vector2 _currentPos;
    [SerializeField, ReadOnly]
    private Vector2 _endPos;
    [SerializeField, ReadOnly]
    private Vector2 _dragVelocity = Vector2.zero;
    public float maxDragVelocity = 1f;
    public float dragFriction = 0.025f;
    public bool lockVertical = true;
    public bool lockHorizontal = false;

    private bool active = false;

    public void StartDrag(Vector2 pos)
    {
        pos = LockPos(pos);
        _startPos = _prevPos = _currentPos = pos;
        _dragVelocity = Vector2.zero;
    }

    private Vector2 LockPos(Vector2 pos)
    {
        if (lockHorizontal)
            pos.x = this.transform.position.x;
        if (lockVertical)
            pos.y = this.transform.position.y;
        return pos;
    }

    public void UpdateDrag(Vector2 pos)
    {
        pos = LockPos(pos);
        _prevPos = _currentPos;
        _currentPos = pos;
        _dragVelocity = _currentPos - _prevPos;

        if (_dragVelocity.x < 0)
        {
            _dragVelocity.x = Mathf.Clamp(_dragVelocity.x, -maxDragVelocity, 0);
        }
        else if (_dragVelocity.x > 0)
        {
            _dragVelocity.x = Mathf.Clamp(_dragVelocity.x, 0, maxDragVelocity);
        }

        if (_dragVelocity.y < 0)
        {
            _dragVelocity.y = Mathf.Clamp(_dragVelocity.y, -maxDragVelocity, 0);
        }
        else if (_dragVelocity.y > 0)
        {
            _dragVelocity.y = Mathf.Clamp(_dragVelocity.y, 0, maxDragVelocity);
        }

        this.transform.position += (Vector3)_dragVelocity;
    }

    public void EndDrag(Vector2 pos)
    {
        pos = LockPos(pos);
        _endPos = pos;
        if (_dragVelocity.magnitude > 0)
            StartCoroutine(DragExit());
    }

    private IEnumerator DragExit()
    {
        while (_dragVelocity != Vector2.zero)
        {
            if (_dragVelocity.x < 0)
            {
                _dragVelocity.x += dragFriction;
                _dragVelocity.x = Mathf.Clamp(_dragVelocity.x, Mathf.NegativeInfinity, 0);
            }
            else if (_dragVelocity.x > 0)
            {
                _dragVelocity.x -= dragFriction;
                _dragVelocity.x = Mathf.Clamp(_dragVelocity.x, 0, Mathf.Infinity);
            }

            if (_dragVelocity.y < 0)
            {
                _dragVelocity.y += dragFriction;
                _dragVelocity.y = Mathf.Clamp(_dragVelocity.y, Mathf.NegativeInfinity, 0);
            }
            else if (_dragVelocity.y > 0)
            {
                _dragVelocity.y -= dragFriction;
                _dragVelocity.y = Mathf.Clamp(_dragVelocity.y, 0, Mathf.Infinity);
            }

            this.transform.position += (Vector3)_dragVelocity;
            yield return new WaitForEndOfFrame();
        }
    }


}
