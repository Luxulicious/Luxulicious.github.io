using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerCount = 0;
    public int playerNum = 0;
    public GameObject rows;
    
    public void Start()
    {
        rows = GameObject.Find("Funnel/Rows");
    }
    

    public void OnGUI()
    {
        if(!PhotonNetwork.IsConnected && GUI.Button(new Rect(10, 10, 50, 20), "Host")) 
            PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCount++;
        
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject row = PhotonNetwork.InstantiateSceneObject("Prefabs/Row", new Vector3(0, 5 - (2*playerCount), 0), Quaternion.identity);
        row.transform.parent = GameObject.Find("Funnel/Rows").transform;

        foreach (var colorable in FindObjectsOfType<RandomlyColorColorables>())
        {
            colorable.SyncRowToPlayer(newPlayer);
        }

        photonView.RPC("SyncGameState", newPlayer, playerCount);
        photonView.RPC("SyncPlayerCount", newPlayer, playerCount);
        photonView.RPC("HideOtherPlayers", RpcTarget.All);
    }

    public override void OnCreatedRoom()
    {
        GameObject row = PhotonNetwork.InstantiateSceneObject("Prefabs/Row", new Vector3(0, 5 - (2 * playerCount), 0), Quaternion.identity);
        row.transform.parent = rows.transform;
    }

    [PunRPC]
    public void SyncPlayerCount(int count)
    {
        playerCount = count;
    }
    
    [PunRPC]
    public void SyncGameState(int count)
    {
        playerNum = count;
    }

    [PunRPC]
    public void HideOtherPlayers()
    {
        Debug.Log($"Hiding all players other than {playerNum}");
        Debug.Log($"Transform rows {rows.transform} :: {rows.name}");
        int i = 0;
        
        foreach (Transform child in FindObjectsOfType<RandomlyColorColorables>().Select(colorables => colorables.transform.parent))
        {
            // if(child.transform.parent == rows.transform)
            child.gameObject.SetActive(true);
        }
        var markIsGek = rows.transform.GetChild(playerNum);
        
        foreach (Transform child in FindObjectsOfType<RandomlyColorColorables>().Select(colorables => colorables.transform.parent))
        {
           // if(child.transform.parent == rows.transform)
                child.gameObject.SetActive(false);
            Debug.Log($"Called {i++} times");
        }

        
        markIsGek.gameObject.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("test", roomOptions, TypedLobby.Default);

        Debug.Log($"SendRate {PhotonNetwork.SendRate}");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}