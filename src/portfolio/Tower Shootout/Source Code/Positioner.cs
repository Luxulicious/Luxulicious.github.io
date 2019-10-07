using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MonoBehaviour
{

    public void SetPosition(Vector2 pos)
    {
        this.transform.position = pos;
    }
}
