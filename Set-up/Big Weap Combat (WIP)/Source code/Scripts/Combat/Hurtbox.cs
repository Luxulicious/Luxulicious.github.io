using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnDamagedEvent : UnityEvent<float> { }
[Serializable]
public class OnKnockbackEvent : UnityEvent<Vector2> { }

public enum HurtboxType { Hittable, Intangible, Invincible }

public class Hurtbox : MonoBehaviour
{
    [Space]
    [Header("Components")]
    private Collider2D _myCol;
    public Combattant self;

    [Space]
    [Header("Debugging")]
    [LabelOverride("Sprite Renderer")]
    public SpriteRenderer sp;
    public bool display = true;
    public Color displayColor = new Color(1, 1, 0, 0.25f);

    [Space]
    public HurtboxType hurtboxType = HurtboxType.Hittable;

    [Space]
    [SerializeField]
    private OnDamagedEvent _onDamagedEvent = new OnDamagedEvent();
    [SerializeField]
    private OnKnockbackEvent _onKnockbackEvent = new OnKnockbackEvent();

    private bool _enabled = true;


    void OnDisable()
    {
        _enabled = false;
    }

    void OnEnable()
    {
        _enabled = true;
    }

    // Use this for initialization
    void Start()
    {
        if (_myCol == null)
            _myCol = this.GetComponent<Collider2D>();
        if (self == null)
            Debug.LogError("An owner for a hurtbox is required.");
        if (sp == null)
            sp = this.GetComponent<SpriteRenderer>();
        if (!_myCol) return;
        if (!_myCol.isTrigger)
            Debug.LogError("Hurtbox is not a trigger.");
    }

    // Update is called once per frame
    void Update()
    {
        sp.enabled = display;
    }

    public void Damage(float damage)
    {
        if (!enabled) return;
        _onDamagedEvent.Invoke(damage);
    }

    public void Knockback(Vector2 knockback)
    {
        if (!enabled) return;
        _onKnockbackEvent.Invoke(knockback);
    }
}
