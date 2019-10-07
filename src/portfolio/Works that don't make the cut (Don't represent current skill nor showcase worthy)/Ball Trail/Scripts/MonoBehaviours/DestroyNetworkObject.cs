using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DestroyNetworkObject : MonoBehaviour
{

	public bool MasterClientOnly = false;
	
	public void DestroyObject(PhotonView view)
	{
		Debug.Log("Should destroy");
		if (MasterClientOnly && !PhotonNetwork.IsMasterClient) return;
		PhotonNetwork.Destroy(view);
	}

	public void TakeOwnershipAndDestroy(PhotonView view) 
	{
		view.RPC("Destroy", RpcTarget.MasterClient);
	}
}
