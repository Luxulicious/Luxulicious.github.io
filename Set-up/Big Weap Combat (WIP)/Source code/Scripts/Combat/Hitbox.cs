using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using Util;

public class Hitbox : MonoBehaviour
{
    //TODO Implement 
    private int _hitboxPriorityId = 0;

    [Header("Damage")]
    [ReadOnly, SerializeField]
    private float _damageModifier = 1f;
    [SerializeField]
    private float _baseDamage = 1f;

    [Header("Knockback"), Space]
    [ReadOnly, SerializeField]
    private float _knockbackModifier = 1f;
    [SerializeField]
    private float _baseKnockback = 10f;
    //TODO Maybe change this to vector instead for consistency
    public float knockbackAngleInDegrees = 45f;
    [Tooltip("Makes the angle relative from where the hitbox connected")]
    public bool launchAngleRelativeToTarget = false;


    [Header("Components"), Space]
    private Collider2D _myCol;
    public Combattant self;
    [SerializeField]
    private List<CombattantType> _unhittableEntityTypes;

    [Space]
    [Header("Debugging")]
    [LabelOverride("Sprite Renderer")]
    public SpriteRenderer sp;
    public bool display = true;
    public Color displayColor = new Color(1, 0, 0, 0.25f);

    private bool _enabled = true;

    void OnDisable()
    {
        _enabled = false;
    }

    void OnEnable()
    {
        _enabled = true;
    }

    void Start()
    {
        if (_myCol == null)
            _myCol = this.GetComponent<Collider2D>();
        if (self == null)
            Debug.LogError("An owner for a hitbox is required.");
        if (sp == null)
            sp = this.GetComponent<SpriteRenderer>();
        if (_myCol)
        {
            if (!_myCol.isTrigger)
                Debug.LogError("Hitbox is not a trigger.");
        }
    }

    void Update()
    {
        if (sp)
            sp.enabled = display;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        var hurtbox = col.GetComponent<Hurtbox>();
        if (!hurtbox) return;
        if (hurtbox.self == self) return;
        if (_unhittableEntityTypes.Contains(hurtbox.self.combattantType)) return;
        if (hurtbox.hurtboxType == HurtboxType.Hittable)
        {
            hurtbox.Damage(_baseDamage * _damageModifier);
            var knockback = CalculateKnockback(_baseKnockback, _knockbackModifier, knockbackAngleInDegrees);
            if (launchAngleRelativeToTarget)
            {
                knockback = MakeKnockbackRelativeToTarget(knockback, col.transform.position);
            }

            hurtbox.Knockback(knockback);
        }
        else if (hurtbox.hurtboxType == HurtboxType.Invincible)
        {
            //TODO
        }
        else if (hurtbox.hurtboxType == HurtboxType.Intangible)
        {
            return;
        }
    }

    private Vector2 MakeKnockbackRelativeToTarget(Vector2 knockback, Vector2 positionTarget)
    {
        var sideSign = Mathf.Sign(transform.position.x - positionTarget.x);
        var dir = knockback.normalized;
        if (sideSign >= 0)
            dir = Vector2.Reflect(dir, Vector2.up);
        knockback = -sideSign * knockback.magnitude * dir;
        return knockback;
    }

    //TODO Move this elsewhere...
    public static Vector2 CalculateKnockback(float baseKnockback, float knockbackModifier,
                                             float knockbackAngleInDegrees)
    {
        return baseKnockback * knockbackModifier * new Vector2(Mathf.Cos(knockbackAngleInDegrees * Mathf.Deg2Rad),
                                                               Mathf.Sin(knockbackAngleInDegrees * Mathf.Deg2Rad));
    }

    public void SetKnockbackModifier(float modifier)
    {
        if (!enabled) return;
        this._knockbackModifier = modifier;
    }

    public void SetDamageModifier(float modifier)
    {
        if (!enabled) return;
        this._damageModifier = modifier;
    }
}


//OLD CODE
/*
TODO Refactor this
if (col.GetComponent<Hurtbox>() != null)
{
    var hurtbox = col.GetComponent<Hurtbox>();
    if (hurtbox.owner == this.owner) return;
    if (hurtbox.hurtboxType == HurtboxType.Hittable)
    {
        hurtbox.Damage(_damage);

        Vector2 knockback;
        if (_positionRelativeAngle)
        {
            Vector2 dir = (transform.position - col.transform.position).normalized;
            knockback = dir * _baseKnockback;
        }
        else
        {
            var sideSign = Mathf.Sign(transform.position.x - col.transform.position.x);
            var dir = (Vector2)(Quaternion.Euler(0, 0, _knockbackAngle) * new Vector2(1, 0));
            if (sideSign >= 0)
                dir = Vector2.Reflect(dir, Vector2.up);
            knockback = -sideSign * _baseKnockback * dir;
        }
        Debug.DrawLine(col.transform.position, (Vector2)col.transform.position + knockback, Color.red);
        hurtbox.Knockback(knockback, _knockbackGrowth, _damage);
    }
}
*/