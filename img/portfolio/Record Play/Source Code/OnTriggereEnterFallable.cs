using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggereEnterFallable : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        var fallable = col.GetComponent<Fallable>();
        if (fallable)
            fallable.Fall();
    }
}
