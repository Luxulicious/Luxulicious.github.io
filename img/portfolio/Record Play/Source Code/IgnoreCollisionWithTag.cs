using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWithTag : MonoBehaviour
{
    public string tag = "Player";
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag(tag))
        {
            Physics2D.IgnoreCollision(col.collider, col.otherCollider, true);
        }
    }
}
