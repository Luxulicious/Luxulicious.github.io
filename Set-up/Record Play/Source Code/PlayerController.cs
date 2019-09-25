using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float verticalMovementSpeed = 5f;
    public float horizontalMovementSpeed = 5f;
    public float autoForwardMovementSpeed = 4f;
    public float distanceFactor = 0.25f;
    public Rigidbody2D rb;
    public Transform relativeToPoint;
    [Range(1, 4)]
    public int playerNumber;

    void FixedUpdate()
    {
        var xAxis = "L_XAxis_" + playerNumber;
        var movementHorizontal = Input.GetAxis(xAxis);
        var yAxis = "L_YAxis_" + playerNumber;
        var movementVertical = -Input.GetAxis(yAxis);

        var dir = relativeToPoint.position - rb.transform.position;
        Debug.DrawLine(this.transform.position, this.transform.position + dir, Color.red);
        var distance = dir.magnitude;

        var rbDirVer = -new Vector2(-dir.y, dir.x);
        rbDirVer = rbDirVer.normalized;
        Debug.DrawLine(rb.transform.position, rb.transform.position + (Vector3)rbDirVer, Color.blue);

        var rbDirHor = (Vector2)dir.normalized;

        if (movementVertical == 0.000000f)
            movementVertical = 0.1f;
        var verVelocity = rbDirVer * movementVertical * verticalMovementSpeed;
        if (verVelocity.magnitude <= autoForwardMovementSpeed && verVelocity.magnitude >= 0)
        {

            verVelocity = verVelocity.normalized * autoForwardMovementSpeed;
        }

        rb.velocity += verVelocity * distance * distanceFactor;

        var horVelocity = -(rbDirHor * movementHorizontal * horizontalMovementSpeed);
        rb.velocity += horVelocity * distance * distanceFactor;

    }
}