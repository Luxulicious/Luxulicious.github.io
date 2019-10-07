using Assets.Scripts.SmartData.Custom.Readers;
using UnityEngine;

public class ScalerLauncher : Launcher
{
    [SerializeField] private RichBoolReader _canLaunch = new RichBoolReader();
    [SerializeField] private Transform _pivot;
    [SerializeField] private float _forceMultiplier = 500f;

    //TODO Min and max force belong more-so in base class
    [SerializeField] private float _minForce = 100f;
    [SerializeField] private float _maxForce = 500f;

    public void LaunchRelativeToDeltaScaleY(ScaleState scaleState)
    {
        var delta = scaleState.currentScale.y - scaleState.prevScale.y;
        LaunchRelativeToFactor(delta);
    }

    public void LaunchRelativeToTimeY(ScaleState scaleState)
    {
        var time = scaleState.timeScalingUpY;
        LaunchRelativeToFactor(time);
    }

    private void LaunchRelativeToFactor(float factor)
    {
        if (!_canLaunch.value) return;
        var dir = -_pivot.transform.up;
        var forceExpected = factor * dir * _forceMultiplier;

        if (forceExpected.magnitude < _minForce)
            Launch(dir * _minForce);
        else if (forceExpected.magnitude > _maxForce)
            Launch(dir * _maxForce);
        else
            Launch(forceExpected);
    }
}