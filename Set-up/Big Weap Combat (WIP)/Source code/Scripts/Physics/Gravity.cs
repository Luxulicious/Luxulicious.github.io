using System.Collections;
using SmartData.SmartBool;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField, Disable] private bool _enabled = true;

    [LabelOverride("Rigidbody"), SerializeField]
    private Rigidbody2D _rb;

    [SerializeField] private BoolReader _grounded;
    public float gravity = 9.81f;
    public float terminalVelocity = 195f / 3.6f;

    void OnDisable()
    {
        StopAllCoroutines();
        _enabled = false;
    }

    void OnEnable()
    {
        _enabled = true;
    }


    void FixedUpdate()
    {
        if (!_enabled) return;
        if (_grounded.value) return;
        if (_rb.velocity.y < -terminalVelocity) return;

        if (_rb.velocity.y > -terminalVelocity)
            _rb.velocity -= new Vector2(0, gravity);
        if (_rb.velocity.y < -terminalVelocity)
            _rb.velocity = new Vector2(_rb.velocity.x, -terminalVelocity);
    }

    public void DisableForFixedUpdates(int amountOfUpdates)
    {
        StartCoroutine(DisableForFixedUpdatesCoroutine(amountOfUpdates));
    }

    public IEnumerator DisableForFixedUpdatesCoroutine(int amountOfUpdates)
    {
        _enabled = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime * amountOfUpdates);
        _enabled = true;
    }
}