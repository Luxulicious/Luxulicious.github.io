using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerExitFallable : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col)
    {
        var fallable = col.GetComponent<Fallable>();
        if (fallable)
            fallable.Fall();
    }

}
