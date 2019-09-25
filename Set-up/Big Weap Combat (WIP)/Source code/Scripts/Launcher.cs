using System;
using SmartData;
using SmartData.SmartFloat;
using UnityEngine;
using UnityEngine.Events;

public class Launcher : MonoBehaviour
{
    public enum ForceApplication
    {
        Incremental,
        Set
    }

    [Serializable]
    public class OnLaunchEvent : UnityEvent<Vector2>
    {
    }

    [SerializeField] private Rigidbody2D _rbToLaunch;

    [Tooltip("If set to \"Set\" velocity will reset before launching again. Incremental will stack velocity.")]
    [SerializeField]
    private OnLaunchEvent _onLaunchEvent = new OnLaunchEvent();

    //TODO Replace this space with a struct or header or whatev...
    [Space] [SerializeField] private ForceApplication _forceApplication = ForceApplication.Set;

    //TODO Only show the following couple of fields when ForceApplication.Set != _forceApplication
    [Tooltip("When enabled it will disable negative counter forces before launching. " +
             "(Prevents launching momentum from being negatively impacted. " +
             "Only really relevant during incremental force application)")]
    [SerializeField]
    private bool _neutralizeCounterForces = true;

    [Tooltip("The absolute maximum speed when launching. " +
             "Value < 0 or null means no maximum speed will be applied." +
             "(Only really relevant during incremental force application)")]
    [SerializeField]
    [ForceHideEvent]
    private FloatReader _maximumSpeed;

    private void Awake()
    {
        if (!_rbToLaunch)
            GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 force)
    {
        //If force to insignificant return
        if (Vector2Extensions.Equals(force, Vector2.zero, 0.00000000001f)) return;
        //Neutralize potential counter-forces if so desired
        if (_neutralizeCounterForces)
            NeutralizeCounterForces(force);
        //Apply the correct force application
        if (_forceApplication == ForceApplication.Incremental)
            ApplyIncrementalForce(force);
        else if (_forceApplication == ForceApplication.Set) ApplySetForce(force);
        //Invoke on launch event
        _onLaunchEvent.Invoke(force);
    }

    private void ApplySetForce(Vector2 force)
    {
        _rbToLaunch.velocity = force;
    }

    private void ApplyIncrementalForce(Vector2 force)
    {
        if (_maximumSpeed != null && _maximumSpeed.value > 0)
        {
            var expectedVelocity = _rbToLaunch.velocity + force;
            if (expectedVelocity.magnitude > _maximumSpeed.value)
                _rbToLaunch.velocity = expectedVelocity.normalized * _maximumSpeed.value;
            else
                _rbToLaunch.velocity += force;
        }
        else
            _rbToLaunch.velocity += force;
    }

    private void NeutralizeCounterForces(Vector2 force)
    {
        if (Math.Abs(force.x) > 0.00000000000f)
            if (Mathf.Sign(_rbToLaunch.velocity.x) != Mathf.Sign(force.x))
                _rbToLaunch.velocity = new Vector2(0, _rbToLaunch.velocity.y);

        if (Math.Abs(force.y) > 0.00000000000f)
            if (Mathf.Sign(_rbToLaunch.velocity.y) != Mathf.Sign(force.y))
                _rbToLaunch.velocity = new Vector2(_rbToLaunch.velocity.x, 0);
    }
}