using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedrawBoxCollider2D : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _col;
    public Transform bottomRight;
    public Transform bottomLeft;
    public Transform topLeft;
    public Transform topRight;

    public void Start()
    {
        if (!_col)
            _col = this.GetComponent<BoxCollider2D>();
    }

    public void Redraw()
    {
        _col.offset = Vector2.zero;
        this.transform.position = (bottomLeft.position + topRight.position) / 2;
        _col.size =
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
