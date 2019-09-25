using UnityEngine;
using Util;

/// <summary>
/// Modified version of (/Credit to): https://github.com/SebLague/2DPlatformer-Tutorial/blob/master/Platformer%20E11/Assets/Scripts/CameraFollow.cs
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    public Transform target;
    public Vector2 focusAreaSize;
    public float verticalOffset;
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;

    private Collider2D _colTarget;
    private FocusArea _focusArea;

    [Header("Gizmos")]
    [SerializeField]
    private bool _showArea = true;
    [SerializeField]
    private Color _showAreaColor = Color.red;

    [Header("Hidden Variables for Debugging"), Space]
    [SerializeField, ReadOnly]
    private float _currentLookAheadX;
    [SerializeField, ReadOnly]
    private float _targetLookAheadX;
    [SerializeField, ReadOnly]
    private float _lookAheadDirX;
    [SerializeField, ReadOnly]
    private float _smoothLookVelocityX;
    [SerializeField, ReadOnly]
    private float _smoothVelocityY;
    [SerializeField, ReadOnly]
    private bool _lookAheadStopped;

    void Start()
    {
        _colTarget = this.target.GetComponent<Collider2D>();
        _focusArea = new FocusArea(_colTarget.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        _focusArea.Update(_colTarget.bounds);

        Vector2 focusPosition = _focusArea.centre + Vector2.up * verticalOffset;

        if (_focusArea.velocity.x != 0)
        {
            _lookAheadDirX = Mathf.Sign(_focusArea.velocity.x);
            if (Mathf.Sign(target.GetComponent<Rigidbody2D>().velocity.x) == Mathf.Sign(_focusArea.velocity.x) && target.GetComponent<Rigidbody2D>().velocity.x != 0)
            {
                _lookAheadStopped = false;
                _targetLookAheadX = _lookAheadDirX * lookAheadDstX;
            }
            else
            {
                if (!_lookAheadStopped)
                {
                    _lookAheadStopped = true;
                    _targetLookAheadX = _currentLookAheadX + (_lookAheadDirX * lookAheadDstX - _currentLookAheadX) / 4f;
                }
            }
        }

        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _targetLookAheadX, ref _smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref _smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    void OnDrawGizmos()
    {
        if (_showArea)
        {
            Gizmos.color = _showAreaColor;
            Gizmos.DrawCube(_focusArea.centre, focusAreaSize);
        }
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
