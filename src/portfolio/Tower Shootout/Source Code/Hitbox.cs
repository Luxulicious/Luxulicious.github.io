using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField, Disable]
    private float _damage = 1f;

    void OnTriggerEnter2D(Collider2D col)
    {
        var hurtbox = col.GetComponent<Hurtbox>();
        if (!hurtbox) return;
        hurtbox.Hit(_damage);
    }
}
