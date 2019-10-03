using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDestroyObjectIfTag : MonoBehaviour
{
    public string tag = "Obstacle";

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag(tag))
            GameObject.Destroy(col.gameObject);
    }
}
