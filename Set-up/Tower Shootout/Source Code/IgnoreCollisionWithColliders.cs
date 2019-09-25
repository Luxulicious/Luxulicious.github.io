using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWithColliders : MonoBehaviour
{
    [SerializeField]
    private Collider2D _myCol;
    public List<Collider2D> collidersToIgnore = new List<Collider2D>();

    void Start()
    {
        if (_myCol == null)
            _myCol = this.GetComponent<Collider2D>();
        foreach (var colliderToIgnore in collidersToIgnore)
        {
            //TODO Replace with group ignore instead
            if (colliderToIgnore != null && _myCol != null)
                Physics2D.IgnoreCollision(colliderToIgnore, _myCol);
        }
    }
}
