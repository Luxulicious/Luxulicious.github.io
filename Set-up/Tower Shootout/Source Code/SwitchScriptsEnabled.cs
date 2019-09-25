using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScriptsEnabled : MonoBehaviour
{
    [SerializeField]
    private List<MonoBehaviour> _scriptsToDisable = new List<MonoBehaviour>();

    public void Disable()
    {
        foreach (var script in _scriptsToDisable)
        {
            script.enabled = false;
        }
    }

    public void Enable()
    {
        foreach (var script in _scriptsToDisable)
        {
            script.enabled = true;
        }
    }
}
