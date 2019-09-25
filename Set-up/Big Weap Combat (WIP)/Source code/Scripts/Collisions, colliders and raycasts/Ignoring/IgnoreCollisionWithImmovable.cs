using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IgnoreCollisionWithImmovable : MonoBehaviour
{

    [SerializeField]
    private Collider2D _myCol;

    void Start()
    {
        if (_myCol == null)
            _myCol = this.GetComponent<Collider2D>();
    }

    //TODO Call this from a static context
    void OnCollisionEnter2D(Collision2D col)
    {
        if (IsTouchingImmovable.IsImmovableObjectCollision(col))
            Physics2D.IgnoreCollision(col.collider, _myCol);
    }
}
