using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject psPrefab;

    public void ExplodeDestructable(Destructable destructable)
    {
        var instance = Instantiate(psPrefab, destructable.destructableRoot.transform.position, Quaternion.identity);
        Destroy(destructable.destructableRoot);
        var ps = instance.GetComponent<ParticleSystem>();
        ps.Play();
        StartCoroutine(InteruptExplode(ps));
    }

    private IEnumerator InteruptExplode(ParticleSystem ps)
    {
        while (ps.isPlaying)
            yield return new WaitForEndOfFrame();
        Destroy(ps.gameObject);
    }
}
