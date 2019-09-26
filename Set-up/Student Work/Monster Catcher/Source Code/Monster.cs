using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using UnityEngine;
using Util;

/// <summary>
/// Contains properties of the monster, such as the color.
/// Also contains the server ended kill-switch for the monster.
/// </summary>
public class Monster : MonoBehaviourPun
{
    public OnColorChangedEvent onColorChangedEvent = new OnColorChangedEvent();
    [SerializeField, ReadOnly]
    private ColorSO _color;

    public ColorSO Color
    {
        get { return _color; }
        set
        {
            var prevColor = _color;
            _color = value;
            if (_color != prevColor)
                onColorChangedEvent.Invoke(_color);
        }
    }

    void Start()
    {
        var colorSo = Resources.Load<ColorSO>("ScriptableObjects/Colors/" + photonView.InstantiationData[0]);
        this.Color = colorSo;
    }

    /// <summary>
    /// Destroys monster via Photon
    /// </summary>
    /// <param name="viewId">Photon View Id</param>
    [PunRPC]
    private void KillMonsterRPC(int viewId)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewId));
    }
}