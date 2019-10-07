using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public void IgnoreCollision2D(Collision2D col)
    {
        Physics2D.IgnoreCollision(col.collider, col.otherCollider);
    }
}
