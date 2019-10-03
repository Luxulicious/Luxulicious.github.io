using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnBeat : MonoBehaviour
{

    public List<Transform> spawnPoints;
    public GameObject prefabSpawnable;
    public Transform attachToParent;

    public void SpawnBeat(Beat beat)
    {
        for (int i = 0; i < beat.notes.Count; i++)
        {
            if (beat.notes[i])
            {
                var mySpawnable =
                    GameObject.Instantiate(prefabSpawnable, spawnPoints[i].position, this.transform.rotation);
                if (attachToParent)
                    mySpawnable.transform.parent = attachToParent;
            }
        }
    }
}
