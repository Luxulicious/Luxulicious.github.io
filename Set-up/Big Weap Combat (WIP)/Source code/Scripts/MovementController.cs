using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField, Disable]
    private bool _enabled = true;
    [Space]
    [SerializeField]
    private Rigidbody2D _rb;
    public float baseSpeed = 1;
    public float growthSpeed = 1;

    delegate void MovementProcessor();

    void OnEnable()
    {
        _enabled = true;
    }

    void OnDisable()
    {
        _enabled = false;
        StopAllCoroutines();
    }

    void Awake()
    {
        if (!_rb)
            _rb = this.GetComponent<Rigidbody2D>();
    }

    public void StartMovingRightSelf()
    {
        if (!_enabled) return;
        StopCoroutine(FixedUpdateMovementCoroutine(MoveRightSelf));
        StartCoroutine(FixedUpdateMovementCoroutine(MoveRightSelf));
    }

    public virtual void MoveRightSelf()
    {
        if (!_enabled) return;
        _rb.velocity = baseSpeed * growthSpeed * _rb.transform.right;
    }

    public virtual void MoveLeft()
    {
        if (!_enabled) return;
        _rb.velocity = new Vector2(-baseSpeed * growthSpeed, _rb.velocity.y);
    }

    public virtual void MoveRight()
    {
        if (!_enabled) return;
        _rb.velocity = new Vector2(baseSpeed * growthSpeed, _rb.velocity.y);
    }

    private IEnumerator FixedUpdateMovementCoroutine(MovementProcessor mp)
    {
        while (true)
        {
            mp();
            yield return new WaitForFixedUpdate();
        }
    }
}