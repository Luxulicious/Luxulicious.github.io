using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour, IPunObservable
{
    public bool spawn = true;
    public List<GameObject> spawnables = new List<GameObject>();
    public List<Transform> spawnpositions = new List<Transform>();
    [Tooltip("In seconds")]
    public float spawnInterval = 5f;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        var valid = true;
        if (!spawnables.Any())
        {
            Debug.LogWarning("No spawnables set. " + this.name + " will not be able to spawn spawnables.");
            valid = false;
        }

        if (!spawnpositions.Any())
        {
            if (!this.transform.GetComponentInChildren<Transform>())
            {
                Debug.LogWarning("No spawnpositions set. " + this.name + " will not be able to spawn spawnables.");
                valid = false;
            }
            else
            {
                foreach (Transform child in this.transform)
                {
                    spawnpositions.Add(child);
                }
            }
        }

        if (valid)
        {

            StartCoroutine(SpawnCoroutine(spawnInterval));
        }
    }

    private IEnumerator SpawnCoroutine(float spawnInterval)
    {
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Spawn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var spawnable = spawnables[Random.Range(0, spawnables.Count)];
        var spawnposition = spawnpositions[Random.Range(0, spawnpositions.Count)];
        PhotonNetwork.InstantiateSceneObject("Prefabs/RandomBall", spawnposition.position, Quaternion.identity);

        //Debug.Log("Spawning ball");

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
