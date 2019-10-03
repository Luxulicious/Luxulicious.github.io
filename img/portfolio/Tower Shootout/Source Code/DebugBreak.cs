using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBreak : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
            Debug.Break();
    }
}
