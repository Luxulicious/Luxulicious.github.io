using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void DebugLog(Vector3 v1, Vector3 v2)
    {
        Debug.Log(v1 + " - " + v2);
    }
}
