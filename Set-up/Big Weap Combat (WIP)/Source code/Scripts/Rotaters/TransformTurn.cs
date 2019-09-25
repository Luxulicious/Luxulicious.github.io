using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTurn : MonoBehaviour
{
    public void TurnHorizontally()
    {
        this.transform.right = -this.transform.right;
    }
}
