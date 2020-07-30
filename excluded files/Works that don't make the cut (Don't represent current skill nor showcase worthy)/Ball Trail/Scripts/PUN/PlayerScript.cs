using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerScript : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private int _playerId;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        if (!photonView.IsMine)
        {
            Debug.Log("PlayerController::CmdRegisterPlayerID -- You are not the local player");
            return;
        }
        Debug.Log("PlayerController::Start -- Started a new local PlayerController");
        LivePlayers.instance.AddLivePlayer(this);
        Debug.Log(LivePlayers.instance.GetLivePlayers().Count);
    }

    private void OnDestroy()
    {
        LivePlayers.instance.RemoveLivePlayer(this);
    }

    public int GetPlayerId()
    {
        return _playerId;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}