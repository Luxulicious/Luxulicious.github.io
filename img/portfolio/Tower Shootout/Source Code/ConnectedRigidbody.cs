using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Util;

[Serializable]
public struct Vector2b
{
    public bool x, y;
}

public class ConnectedRigidbody : MonoBehaviour
{
    
    [LabelOverride("Parent Rigidbody"), SerializeField]
    private Rigidbody2D _rb;
    [LabelOverride("Descendant Rigidbodies"), SerializeField]
    private List<Rigidbody2D> _rbChildren = new List<Rigidbody2D>();

    public Vector2b inheritVelocity;
    //TODO Scale and velocity maybe



    void FixedUpdate()
    {
        if(inheritVelocity.x)
            _rbChildren.ForEach(x => x.velocity = new Vector2(_rb.velocity.x, x.velocity.y));
        if (inheritVelocity.y)
            _rbChildren.ForEach(x => x.velocity = new Vector2(x.velocity.x, _rb.velocity.y));
    }


    public void SetInheritVelocityX(bool inheritVelocityX)
    {
        inheritVelocity.x = inheritVelocityX;
    }

    public void SetInheritVelocityY(bool inheritVelocityY)
    {
        inheritVelocity.y = inheritVelocityY;
    }


    public void SetVelocities(Vector2 vel)
    {
        _rbChildren.ForEach(x => x.velocity = vel);
    }
}
