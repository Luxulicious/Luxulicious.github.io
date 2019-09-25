using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Adds a compensating torque to counter gravity
/// </summary>
public class AntiGravityTorque : MonoBehaviour
{
    [Disable, SerializeField] private bool _valid = false;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Gravity _gravity;
    [SerializeField, Tooltip("Masses where gravity is being exerted onto")]
    private List<Mass> _masses;

    [SerializeField] private Transform _topMiddleCoordinate;

    void Awake()
    {
        if (!_rb)
            _rb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!this._valid) return;

        var angle = GetAngleInRadians();
        var hypotenuse = GetHypotenuse();
        var opposite = GetOpposite(hypotenuse, angle);
        var adjacent = GetAdjacent(hypotenuse, opposite);

        var pointA = (Vector2) _rb.transform.position;
        var pointB = pointA + hypotenuse * (Vector2) _rb.transform.up;
        var pointC = pointB + opposite * Vector2.right;
        var pointD = pointC + adjacent * Vector2.up;
        Debug.DrawLine(pointA, pointB, Color.red);
        Debug.DrawLine(pointB, pointC, Color.green);
        Debug.DrawLine(pointC, pointD, Color.blue);

        //T = f * r
        var force = _gravity.gravity * _masses.Sum(x => x.mass) * 1/Time.fixedDeltaTime;
        var radius = opposite;
        var torque = force * radius;
        _rb.AddTorque(torque);
        var adjustTorque = torque / 1000;
        Debug.DrawLine(_rb.transform.position, _rb.transform.position + adjustTorque * transform.right, Color.magenta);
    }

    private static float GetAdjacent(float hypotenuse, float opposite)
    {
        return Mathf.Sqrt(Mathf.Pow(hypotenuse, 2) - Mathf.Pow(opposite, 2));
    }

    private static float GetOpposite(float hypotenuse, float angle)
    {
        return hypotenuse * Mathf.Sin(angle);
    }

    private float GetHypotenuse()
    {
        return Vector2.Distance(_topMiddleCoordinate.position, _rb.worldCenterOfMass);
    }

    private float GetRotationDirection()
    {
        return Mathf.Sign(_rb.transform.eulerAngles.z);
    }

    public void SetValid(bool valid)
    {
        this._valid = valid;
    }


    /// <summary>
    /// Get's the degree from the corner of the weapon parallel to the horizontal axis
    /// </summary>
    /// <returns></returns>
    public float GetAngleInRadians()
    {
        return Mathf.Atan2(-_rb.transform.up.x, -_rb.transform.up.y);
    }
}