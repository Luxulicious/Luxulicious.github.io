using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

//TODO Try and apply delegates here as to ensure easier to read/maintain code...
public class HitstunReceiver : MonoBehaviour
{
    private bool enabled = true;
    [SerializeField] private Rigidbody2D _rb;

    [Serializable]
    struct Rigidbody2DSettings
    {
        [ReadOnly] public float gravityScale, angularDrag, drag;
    }

    [SerializeField]
    private Rigidbody2DSettings prevRbSettings;
    [SerializeField]
    private List<MonoBehaviour> _scriptsToDisable = new List<MonoBehaviour>();
    [SerializeField, ReadOnly]
    [Tooltip("Keeps track of the scripts previous state that will be disabled.")]
    private Dictionary<MonoBehaviour, bool> _scriptsToDisableOriginalStates = new Dictionary<MonoBehaviour, bool>();
    [SerializeField]
    private List<GameObject> _gameObjectsToDeactivate = new List<GameObject>();
    [SerializeField, ReadOnly]
    [Tooltip("Keeps track of the gameobjects previous states that will be deactivated.")]
    private Dictionary<GameObject, bool> _gameObjectsToDeactivateOriginalStates = new Dictionary<GameObject, bool>();

    [SerializeField, ReadOnly] private bool _inHitStun = false;

    [Header("Debug")]
    [SerializeField, LabelOverride("SpriteRenderer hitstun")]
    private SpriteRenderer sp;

    void Start()
    {
        if (!sp)
            sp = this.GetComponent<SpriteRenderer>();
        if (sp)
            oColor = sp.color;
        SetScriptsToDisableOriginalStates();
    }

    void Update()
    {
        if (sp)
            sp.color = _inHitStun ? Color.red : oColor;
    }

    void OnEnable()
    {
        enabled = true;
    }

    void OnDisable()
    {
        enabled = false;
        StopAllCoroutines();
        _inHitStun = false;
        EnableInternalForces();
        RevertScriptState();
        RevertGameObjectsToDeactivate();
    }

    public void HitStun(int frames)
    {
        if (!enabled) return;
        if (!_inHitStun)
        {
            SavePrevRbSettings();
            SetScriptsToDisableOriginalStates();
            SetGameObjectsToDeactivateOriginalStates();
        }
        StartCoroutine(HitStunCoroutine(frames));
    }

    private void SetScriptsToDisableOriginalStates()
    {
        _scriptsToDisableOriginalStates.Clear();
        _scriptsToDisable.ForEach(x => _scriptsToDisableOriginalStates.Add(x, x.enabled));
    }

    private void SetGameObjectsToDeactivateOriginalStates()
    {
        _gameObjectsToDeactivateOriginalStates.Clear();
        _gameObjectsToDeactivate.ForEach(x => _gameObjectsToDeactivateOriginalStates.Add(x, x.activeSelf));
    }

    private void SavePrevRbSettings()
    {
        prevRbSettings.gravityScale = _rb.gravityScale;
        prevRbSettings.angularDrag = _rb.angularDrag;
        prevRbSettings.drag = _rb.drag;
    }

    //TODO Remove these color sheninigans
    private Color oColor;

    private IEnumerator HitStunCoroutine(int frames)
    {
        _inHitStun = true;
        DisableInternalForces();
        DisableScripts();
        DeactivateGameObjectsToDeactivate();
        yield return new WaitForSeconds(1 / 60f * frames);
        _inHitStun = false;
        EnableInternalForces();
        RevertScriptState();
        RevertGameObjectsToDeactivate();
        yield return null;
    }

    private void DeactivateGameObjectsToDeactivate()
    {
        _gameObjectsToDeactivate.ForEach(x => x.SetActive(false));
    }

    private void RevertGameObjectsToDeactivate()
    {
        foreach (var gameObjectToDeactivateOriginalStateData in _gameObjectsToDeactivateOriginalStates)
        {
            gameObjectToDeactivateOriginalStateData.Key.SetActive(gameObjectToDeactivateOriginalStateData.Value);
        }
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

    private void EnableInternalForces()
    {
        if (_rb)
            LoadPrevRbSettings();
    }

    private void LoadPrevRbSettings()
    {
        _rb.gravityScale = prevRbSettings.gravityScale;
        _rb.angularDrag = prevRbSettings.angularDrag;
        _rb.drag = prevRbSettings.drag;
    }

    private void DisableInternalForces()
    {
        if (_rb)
        {
            NullifyRbSettings();
        }
    }

    private void NullifyRbSettings()
    {
        _rb.gravityScale = 0;
        _rb.drag = 0;
        _rb.drag = 0;
    }
}
