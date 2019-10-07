using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scales a transform up or down incrementally in 2D space
/// </summary>
public class IncrementalScaler : MonoBehaviour
{
    [SerializeField] private ScaleSettings _scaleSettings = new ScaleSettings();
    [SerializeField] private ScaleState _scaleState = new ScaleState();
    [SerializeField] private ScaleEvents _scaleEvents = new ScaleEvents();

    void Awake()
    {
        _scaleState.prevScale = this.transform.localScale;
        _scaleState.currentScale = this.transform.localScale;
    }

    void FixedUpdate()
    {
        ScaleY();
    }

    public void StartScalingUp()
    {
        EndScalingDown();
        _scaleState.scalingUpX = true;
        _scaleState.scalingUpY = true;
    }

    public void EndScalingUp()
    {
        _scaleState.scalingUpX = false;
        _scaleState.scalingUpY = false;
    }

    public void StartScalingDown()
    {
        EndScalingUp();
        _scaleState.scalingDownX = true;
        _scaleState.scalingDownY = true;
    }

    public void EndScalingDown()
    {
        _scaleState.scalingDownX = false;
        _scaleState.scalingDownY = false;
    }

    private void ScaleY()
    {
        if (_scaleState.scalingUpY && _scaleState.scalingDownY)
        {
            _scaleState.scalingUpY = false;
            _scaleState.scalingDownY = false;
            if(_scaleState.timeScalingUpY > 0 && _scaleState.prevScale.y < _scaleState.currentScale.y)
                _scaleEvents.InvokeOnScalingUpYEndedEvent(_scaleState);
            _scaleState.timeScalingUpY = 0;
            _scaleState.timeScalingDownY = 0;
        }
        else if (_scaleState.scalingUpY)
        {
            if (_scaleState.timeScalingDownY > 0 && _scaleState.prevScale.y > _scaleState.currentScale.y)
            {
                //TODO
                //_scaleEvents.onScalingDownYEndedEvent.Invoke(_scaleState.prevScale, _scaleState.currentScale);
                _scaleState.timeScalingDownY = 0;
            }
            ScalingUpY();
        }
        else if (_scaleState.scalingDownY)
        {
            if (_scaleState.timeScalingUpY > 0 && _scaleState.prevScale.y < _scaleState.currentScale.y)
            {
                _scaleEvents.InvokeOnScalingUpYEndedEvent(_scaleState);
                _scaleState.timeScalingUpY = 0;
            }
            ScalingDownY();
        }
        else
        {
            if (_scaleState.timeScalingUpY > 0 && _scaleState.prevScale.y < _scaleState.currentScale.y)
            {
                _scaleEvents.onScalingUpYEndedEvent.Invoke(_scaleState);
                _scaleState.timeScalingUpY = 0;
            }

            if (_scaleState.timeScalingDownY > 0 && _scaleState.prevScale.y > _scaleState.currentScale.y)
            {
                //TODO
                //_scaleEvents.onScalingDownYEndedEvent.Invoke(_scaleState.prevScale, _scaleState.currentScale);
                _scaleState.timeScalingDownY = 0;
            }
        }
    }

    private void ScalingDownY()
    {
        //If at max size
        if (_scaleState.currentScale.y <= _scaleSettings.minScale.y)
        {
            _scaleState.scalingUpY = false;
            return;
        }

        _scaleState.timeScalingDownY += Time.fixedDeltaTime;

        _scaleState.prevScale.y = this.transform.localScale.y;
        ScaleTransformDownY();
        _scaleState.currentScale.y = this.transform.localScale.y;


        //If shrunk
        if (_scaleState.prevScale.y > _scaleState.currentScale.y)
        {
            //TODO
            //_scaleEvents.InvokeOnScalingDownYEvent(_scaleState);
        }
        else
        {
            _scaleState.timeScalingDownY = 0;
            //TODO
            /*if (_scaleState.scalingDownY)
                   _scaleEvents.onScalingDownYEndedEvent.Invoke(_scaleState);*/
            _scaleState.scalingUpY = false;
        }
    }

    private void ScalingUpY()
    {
        //If at max size
        if (_scaleState.currentScale.y >= _scaleSettings.maxScale.y)
        {
            _scaleState.scalingUpY = false;
            return;
        }

        _scaleState.timeScalingUpY += Time.fixedDeltaTime;

        _scaleState.prevScale.y = this.transform.localScale.y;
        ScaleTransformUpY();
        _scaleState.currentScale.y = this.transform.localScale.y;

        //If grew
        if (_scaleState.prevScale.y < _scaleState.currentScale.y)
        {
            _scaleEvents.InvokeOnScalingUpYEvent(_scaleState);
        }
        else
        {
            _scaleState.timeScalingUpY = 0;
            if (_scaleState.scalingUpY)
                _scaleEvents.onScalingUpYEndedEvent.Invoke(_scaleState);
            _scaleState.scalingUpY = false;
        }
    }

    private void ScaleTransformUpY()
    {
        //Apply new scale
        this.transform.localScale = new Vector3
        (
            this.transform.localScale.x,
            this.transform.localScale.y + _scaleSettings.scaleUpSpeed.y * _scaleState.timeScalingUpY,
            this.transform.localScale.z
        );

        //Clamp the new scale to min- & max values
        this.transform.localScale = this.transform.localScale.Clamp(_scaleSettings.minScale.x,
            _scaleSettings.maxScale.x, _scaleSettings.minScale.y,
            _scaleSettings.maxScale.y);
    }

    private void ScaleTransformDownY()
    {
        //Apply new scale
        this.transform.localScale = new Vector3
        (
            this.transform.localScale.x,
            this.transform.localScale.y - _scaleSettings.scaleDownSpeed.y * _scaleState.timeScalingDownY,
            this.transform.localScale.z
        );

        //Clamp the new scale to min- & max values
        this.transform.localScale = this.transform.localScale.Clamp(_scaleSettings.minScale.x,
            _scaleSettings.maxScale.x, _scaleSettings.minScale.y,
            _scaleSettings.maxScale.y);
    }
}