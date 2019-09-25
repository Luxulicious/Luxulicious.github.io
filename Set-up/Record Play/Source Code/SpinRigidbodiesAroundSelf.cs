using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRigidbodiesAroundSelf : MonoBehaviour
{
    public List<Rigidbody2D> rbs = new List<Rigidbody2D>();
    public float force = 1f;
    public float distanceFactor = 0.1f;
    public bool adjustRotation = false;

    // Update is called once per frame
    protected void FixedUpdate()
    {
        rbs.RemoveAll(x => x == null);
        foreach (var rb in rbs)
        {
            var dir = rb.transform.position - this.transform.position;
            var rbDir = -new Vector2(-dir.y, dir.x);
            rbDir = rbDir.normalized;
            var distance = dir.magnitude;
            var movement = rbDir * force * distance * distanceFactor;
            Debug.DrawLine(rb.position, rb.position + movement, Color.magenta);
            rb.velocity = movement;
            if (adjustRotation)
            {
                var angle = Mathf.Atan2(rbDir.y, rbDir.x) * Mathf.Rad2Deg - 90;
                rb.rotation = angle;
            }
        }
    }
}
