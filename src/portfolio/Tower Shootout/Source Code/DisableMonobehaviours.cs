using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;


public class DisableMonobehaviours : MonoBehaviour
{
    [SerializeField]
    private List<MonoBehaviour> _scriptsToDisable = new List<MonoBehaviour>();
    [SerializeField, ReadOnly]
    [Tooltip("Keeps track of the scripts previous state that will be disabled.")]
    private Dictionary<MonoBehaviour, bool> _scriptsToDisableOriginalStates = new Dictionary<MonoBehaviour, bool>();
    [SerializeField, ReadOnly]
    private bool _disabling;


    void Start()
    {
        SetScriptsToDisableOriginalStates();
    }

    void OnDisable()
    {
        RevertScriptState();
    }

    private void SetScriptsToDisableOriginalStates()
    {
        _scriptsToDisableOriginalStates.Clear();
        _scriptsToDisable.ForEach(x => _scriptsToDisableOriginalStates.Add(x, x.enabled));
    }

    private void DisableScripts()
    {
        foreach (var script in _scriptsToDisable)
        {
            script.enabled = false;
        }
    }

    private void RevertScriptState()
    {
        foreach (var scriptToDisableOriginalStateData in _scriptsToDisableOriginalStates)
        {
            scriptToDisableOriginalStateData.Key.enabled = scriptToDisableOriginalStateData.Value;
        }
    }
    private IEnumerator DisableCoroutine(int frames)
    {
        _disabling = true;
        DisableScripts();
        yield return new WaitForSeconds(1 / 60f * frames);
        _disabling = false;
        RevertScriptState();
        yield return null;
    }

    public void Disable()
    {
        if (!_disabling)
        {
            SetScriptsToDisableOriginalStates();
        }
        StopAllCoroutines();
        DisableScripts();
    }

    public void DisableForFrames(int frames)
    {
        if (!_disabling)
        {
            SetScriptsToDisableOriginalStates();
        }
        StartCoroutine(DisableCoroutine(frames));
    }
}