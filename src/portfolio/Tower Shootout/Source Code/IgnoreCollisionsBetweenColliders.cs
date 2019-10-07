using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionsBetweenColliders : MonoBehaviour
{

    public List<Collider2D> collidersToIgnoreEachother = new List<Collider2D>();

    void Start()
    {
        //TODO Has some unnecessary iterations to fix
        for (int i = 0; i < collidersToIgnoreEachother.Count; i++)
        {
            for (int j = 0; j < collidersToIgnoreEachother.Count; j++)
            {
                if (i == j)
                    continue;
                Physics2D.IgnoreCollision(collidersToIgnoreEachother[i], collidersToIgnoreEachother[j], true);
            }
        }
    }
}
