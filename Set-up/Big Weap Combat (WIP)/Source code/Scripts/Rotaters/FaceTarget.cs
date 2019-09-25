using UnityEngine;
using Util;

public class FaceTarget : MonoBehaviour
{
    [LabelOverride("Rigidbody 2D")]
    [SerializeField]
    private Rigidbody2D _rb;

    public bool immediateRotation = false;
    public float maxRotationSpeed = 50f;
    [SerializeField]
    private float _rotationSpeed = 0f;

    [SerializeField, ReadOnly]
    private float _targetAngle;

    [SerializeField, ReadOnly]
    private Vector2? _targetPos = null;

    void Start()
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
    }

    public void FacePosition(Vector2 pos)
    {
        if (!_rb) return;
        if (!this.isActiveAndEnabled) return;

        _rb.angularVelocity = 0;
        _targetPos = pos;
        _targetAngle = Mathf.Atan2(this.transform.position.y - _targetPos.Value.y,
                                   this.transform.position.x - _targetPos.Value.x) * Mathf.Rad2Deg - 90 + 180;
        var deltaRotation = _targetAngle;
        if (!immediateRotation)
        {
             deltaRotation = Mathf.LerpAngle(_rb.rotation, _targetAngle, Time.fixedDeltaTime * Mathf.Clamp(_rotationSpeed, 0, maxRotationSpeed));
        }
        _rb.MoveRotation(deltaRotation);
    }

    void FixedUpdate()
    {
        _rb.angularVelocity = 0;
    }
}