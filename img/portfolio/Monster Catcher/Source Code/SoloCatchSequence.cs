using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Creates a sequence of events that can be invoked when a trap drops for solo players
/// </summary>
public class SoloCatchSequence : MonoBehaviourPun
{
    public GameObject catchInfoGameObject;
    private Sprite successSprite;
    private Sprite failSprite;
    private bool isDropping = false;

    void Start()
    {
        successSprite = Resources.Load<Sprite>("Textures/checkmark-png-28");
        failSprite = Resources.Load<Sprite>("Textures/wrong-icon-15");
    }

    /// <summary>
    /// Starts a coroutine of the catch sequence
    /// </summary>
    /// <param name="playerObj">Player that dropped the trap</param>
    /// <param name="monsterObj">Nullable, if left null monster catch failed</param>
    public void AttemptCatch(GameObject playerObj, GameObject monsterObj)
    {
        if (!isDropping)
        {
            isDropping = true;
            StartCoroutine(monsterObj ? Catch(playerObj, monsterObj) : Miss(playerObj));
        }
    }

    private IEnumerator Catch(GameObject playerObj, GameObject monsterObj)
    {    
        photonView.RPC("SetCatchInfoIcon", RpcTarget.All, true);
        PlayerController playCon = GetComponent<PlayerController>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        playCon.SetControlsEnabled(false);
        rb.velocity = new Vector2(0, 0);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerObj.transform.position = new Vector2(monsterObj.transform.position.x > 0 ? monsterObj.transform.position.x - 1 : monsterObj.transform.position.x + 1,
            monsterObj.transform.position.y > 0 ? monsterObj.transform.position.y - 6 : monsterObj.transform.position.y + 6);

        photonView.RPC("PlayCatchAnim", RpcTarget.All, monsterObj.GetComponent<PhotonView>().ViewID);
        yield return new WaitForSeconds(0.95f);
        monsterObj.GetPhotonView().RPC("KillMonsterRPC", RpcTarget.MasterClient, monsterObj.GetComponent<PhotonView>().ViewID);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playCon.SetControlsEnabled(true);
        photonView.RPC("ClearCatchInfoIcon", RpcTarget.All);
        isDropping = false;
    }

    private IEnumerator Miss(GameObject playerObj)
    {
        photonView.RPC("SetCatchInfoIcon", RpcTarget.All, false);
        PlayerController playCon = GetComponent<PlayerController>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        playCon.SetControlsEnabled(false);
        rb.velocity = new Vector2(0, 0);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        photonView.RPC("PlayCatchAnim", RpcTarget.All, -1337);
        yield return new WaitForSeconds(0.95f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playCon.SetControlsEnabled(true);
        photonView.RPC("ClearCatchInfoIcon", RpcTarget.All);
        isDropping = false;
    }

    [PunRPC]
    private void SetCatchInfoIcon(bool hit)
    {
        catchInfoGameObject.GetComponent<SpriteRenderer>().sprite = hit ? successSprite : failSprite;
        catchInfoGameObject.transform.localScale = new Vector3(hit ? 0.2f : 0.3f, hit ? 0.2f : 0.3f, 1);
    }

    [PunRPC]
    private void ClearCatchInfoIcon()
    {
        catchInfoGameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    [PunRPC]
    private void PlayCatchAnim(int monsterViewId)
    {
        if (monsterViewId >= 0)
        {
            PhotonView.Find(monsterViewId).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        GetComponentInChildren<Animator>().SetTrigger("Catching");
    }
}
