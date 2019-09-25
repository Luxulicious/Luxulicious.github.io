using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class RandomColorable : Colorable, IPunObservable
{
    protected PhotonView photonView;
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!sp)
            sp = this.GetComponent<SpriteRenderer>();
        SetRandomColor();
        
        if(PhotonNetwork.IsMasterClient) 
            photonView.RPC("SetColorRPC", RpcTarget.Others, color.collisionId);
    }

    public void SetRandomColor()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        var randomAvailableColor = AvailableColors.instance.GetRandomAvailableColor();
        this.SetColorable(randomAvailableColor);
    }

    [PunRPC] public void SetColorRPC(int collisionId)
    {
        SetColorable(AvailableColors.instance.GetColorById(collisionId));
    }

    [PunRPC]
    public void Destroy()
    {
        PhotonNetwork.Destroy(photonView);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // Method not really needed, but needs to be overridden due to IPunObservable
    }
}
