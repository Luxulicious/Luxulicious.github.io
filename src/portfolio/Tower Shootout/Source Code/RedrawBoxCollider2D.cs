using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedrawBoxCollider2D : MonoBehaviour
{
    private BoxCollider2D collider;
    public Transform bottomRight;
    public Transform bottomLeft;
    public Transform topLeft;
    public Transform topRight;

    public void Start()
    {
        if (!collider)
            collider = this.GetComponent<BoxCollider2D>();
    }

    public void Redraw()
    {
        collider.offset = Vector2.zero;
        this.transform.position = (bottomLeft.position + topRight.position) / 2;
        collider.size =
            new Vector2
            (
                Vector2.Distance(bottomLeft.position, bottomRight.position),
                Vector2.Distance(bottomRight.position, topRight.position)
            );
    }

    void FixedUpdate()
    {
        Redraw();
    }
}
