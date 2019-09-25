using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/*
 * TODO Make sure they dont go out of bounds or make an imperfect circle
 * by adjusting their position to be the distance from the start.
 * Cranking up the physics update is sheit!
 */
public class SpinSpawnablesAroundSelf : SpinRigidbodiesAroundSelf
{
    void Start()
    {
        Spawnable.onSpawnedEvent.AddListener(AddSpawnableToRbs);
        Spawnable.onDespawnedEvent.AddListener(RemoveSpawnableFromRbs);
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void AddSpawnableToRbs(Spawnable spawnable)
    {
        rbs.Add(spawnable.GetComponent<Rigidbody2D>());
    }

    void RemoveSpawnableFromRbs(Spawnable spawnable)
    {
        rbs.Remove(spawnable.GetComponent<Rigidbody2D>());
    }
}

