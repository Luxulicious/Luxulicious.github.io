using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotateParentPosition : MonoBehaviour
{

    public Transform parent;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -gameObject.transform.rotation.z);
    }
}
