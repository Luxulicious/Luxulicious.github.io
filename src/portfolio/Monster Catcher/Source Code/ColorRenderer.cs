using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Util;

/// <summary>
/// Changes the sprite renderer's color based on ColorSO
/// </summary>
public class ColorRenderer : MonoBehaviourPun
{
    [SerializeField, LabelOverride("Renderer")]
    private SpriteRenderer _sp;
    [SerializeField, ReadOnly]
    private ColorSO _color;

    void Start ()
    {
        if (!_sp)
            _sp = this.GetComponent<SpriteRenderer>();
    }

    public void SetColor(ColorSO color)
    {
        this._color = color;
        _sp.color = color.color;
    }

    public ColorSO GetColor()
    {
        return this._color;
    }
}
