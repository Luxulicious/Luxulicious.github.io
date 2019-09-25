using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsRendering : MonoBehaviour
{

    public List<SpriteRenderer> sps = new List<SpriteRenderer>();

    void Start()
    {
        if (!sps.Any())
            this.sps = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    void Update()
    {
        if (!CheckRenderers())
            Debug.Log("Something invisible happened!");
    }

    public bool CheckRenderers()
    {
        foreach (var sp in sps)
        {
            // If at least one render is visible, return true
            if (sp.isVisible)
            {
                return true;
            }
        }
        // Otherwise, the object is invisible
        return false;
    }


}
