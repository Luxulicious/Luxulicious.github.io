using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDestructable : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var destructable = col.GetComponent<Destructable>();
        if (destructable)
        {
            destructable.Destroy();
        }
    }
}
