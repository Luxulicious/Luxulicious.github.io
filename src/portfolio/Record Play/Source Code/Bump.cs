using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bump : MonoBehaviour
{
    public Rigidbody2D rb;
    public float minForce = 1f;
    public float bumpiness = 1f;

    void OnCollisionEnter2D(Collision2D col)
    {
        var bumpable = col.gameObject.GetComponent<Bumpable>();
        if (bumpable)
        {
            var firstContact = col.contacts[0];
            var normal = firstContact.normal;
            var normalImpulse = firstContact.normalImpulse;
            if (normalImpulse > minForce)
            {
                bumpable.rb.velocity += normal * (normalImpulse * bumpiness);
            }
        }
    }
}

