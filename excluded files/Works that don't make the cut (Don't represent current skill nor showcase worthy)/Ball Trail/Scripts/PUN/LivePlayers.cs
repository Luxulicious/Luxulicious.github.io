using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class LivePlayers : NetworkBehaviour
{
    private List<PlayerScript> _livePlayers = new List<PlayerScript>();
    public static LivePlayers instance;

    void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Debug.LogError(this.GetType() + "::" + "Awake" + " -- Only one instance of " + this.GetType() + " is allowed at a time");
            Destroy(this.gameObject);
        }
    }

    public void AddLivePlayer(PlayerScript player)
    {
        if (_livePlayers.Any(x => x.GetPlayerId() == player.GetPlayerId())) return;
        _livePlayers.Add(player);
    }

    public void RemoveLivePlayer(PlayerScript player)
    {
        if (!(_livePlayers.Any(x => x.GetPlayerId() == player.GetPlayerId()))) return;
        _livePlayers.Remove(player);
    }

    public List<PlayerScript> GetLivePlayers()
    {
        return _livePlayers;
    }
}

