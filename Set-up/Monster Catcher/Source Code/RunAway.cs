using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Thrown when moving away from threat
/// </summary>
[Serializable]
public class IsRunningAwayEvent : UnityEvent<bool> { }

/// <summary>
/// Behaviour to make this game object move away when coming in proximity of threat
/// </summary>
public class RunAway : MonoBehaviour {

    public Rigidbody2D rb;
    public List<Transform> froms;
    public float minDistance;
    public float runSpeed = 10f;
    public IsRunningAwayEvent isRunningAwayEvent = new IsRunningAwayEvent();
    SpriteRenderer spriteRndr;

    // Use this for initialization
    void Start () {
        if (!froms.Any())
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                froms.Add(player.transform);
            }
        }
        spriteRndr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;
        foreach (var from in froms)
        {
            if (Vector2.Distance(this.transform.position, from.position) < minDistance)
            {
                movement = (movement + ((Vector2)this.transform.position - (Vector2)from.position).normalized).normalized;
            }
        }
        isRunningAwayEvent.Invoke(movement.magnitude > 0);
        rb.velocity = (runSpeed * Time.deltaTime * movement);
        if(movement.x < 0)
        {
            spriteRndr.flipX = true;
        }
        else if(movement.x > 0)
        {
            spriteRndr.flipX = false;
        }
    }

}
