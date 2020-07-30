using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RandomlyColorColorables : MonoBehaviour, IPunObservable
{
    public List<Colorable> colorables = new List<Colorable>();
    private PhotonView photonView;

    void Start()
    {
        if (transform.parent.parent == null)
        {
            transform.parent.parent = GameObject.Find("Funnel/Rows").transform;
        }
        photonView = GetComponent<PhotonView>();
        
        if (!colorables.Any())
            this.colorables = this.GetComponentsInChildren<Colorable>().ToList();
        foreach (var colorable in colorables)
        {
            var randomAvailableColor = AvailableColors.instance.GetRandomAvailableColor();
            colorable.SetColorable(randomAvailableColor);
        }
    }

    public void SyncRowToPlayer(Player player)
    {
        for (var i = 0; i < colorables.Count; i++)
        {
            photonView.RPC("SyncColorable", RpcTarget.Others, i, colorables[i].color.collisionId);
        }
    }

    [PunRPC]
    public void SyncColorable(int idx, int collisionId)
    {
        colorables[idx].SetColorable(AvailableColors.instance.GetColorById(collisionId));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
//        throw new System.NotImplementedException();
    }
}
