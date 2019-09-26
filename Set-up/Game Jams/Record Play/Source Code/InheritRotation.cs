using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritRotation : MonoBehaviour
{
    public Transform parent;
    public Transform child;

    void Update()
    {
        child.transform.rotation = parent.transform.rotation;
    }

}
