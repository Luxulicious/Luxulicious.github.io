using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterIfTagDestroySelf : MonoBehaviour
{
    public string tag = "DespawnArea";

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(tag))
            Destroy(this.gameObject);
    }

}
