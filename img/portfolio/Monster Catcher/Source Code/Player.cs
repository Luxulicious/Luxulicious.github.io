using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Util;

/// <summary>
/// Thrown when a color has changed on this game object
/// Param: The new ColorSO
/// </summary>
[Serializable]
public class OnColorChangedEvent : UnityEvent<ColorSO>
{
}

/// <summary>
/// In essence an extension of Photon.Player with other attributes such as color.
/// </summary>
[Serializable]
public class Player : MonoBehaviourPun
{
    private static List<Player> _livePlayers = new List<Player>();
    private bool _instancedOnce = false;
    public OnColorChangedEvent onColorChangedEvent = new OnColorChangedEvent();
    [SerializeField, ReadOnly]
    public int actorNumber;
    [SerializeField, ReadOnly]
    private ColorSO _color;

    void OnEnable()
    {
        AddOrUpdateLivePlayer(this);
    }

   public ColorSO Color
    {
        get { return _color; }
        set
        {
            var prevColor = _color;
            _color = value;
            if (_color != prevColor)
            {
                onColorChangedEvent.Invoke(_color);
            }
        }
    }

    void Start()
    {
        //Debug.Log("Live players: " + _livePlayers.Count);
        this.actorNumber = (int)photonView.InstantiationData[0];
        var colorSo = Resources.Load<ColorSO>("ScriptableObjects/Colors/" + photonView.InstantiationData[1]);
        this.Color = colorSo;
        AddOrUpdateLivePlayer(this);
    }

    void OnDisable()
    {
        RemoveLivePlayer(this);
    }

    public static void AddOrUpdateLivePlayer(Player player)
    {
        if (_livePlayers.Any(x => x.photonView.ViewID == player.photonView.ViewID))
            _livePlayers.RemoveAll(x => x.photonView.ViewID == player.photonView.ViewID); 
        _livePlayers.Add(player);
    }

    public static void RemoveLivePlayer(Player player)
    {
        _livePlayers.RemoveAll(x => x.photonView.ViewID == player.photonView.ViewID);
    }

    public static void RemoveLivePlayer(int playerId)
    {
        _livePlayers.RemoveAll(x => x.photonView.ViewID == playerId);
    }

    public static List<Player> GetLivePlayers()
    {
        return _livePlayers;
    }

    /// <summary>
    /// Returns a list of team players. (Excluding self!)
    /// </summary>
    /// <param name="player"></param>
    /// <returns>List of team players. (Excluding self!)</returns>
    public static List<Player> GetLiveTeamPlayers(Player player)
    {
        return _livePlayers.FindAll(x => x.Color.id == player.Color.id && x.photonView.ViewID != player.photonView.ViewID);
    }
}