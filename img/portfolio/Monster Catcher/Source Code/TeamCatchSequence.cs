using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;


/// <summary>
/// Creates a sequence of events that can be invoked when a trap drops for teams
/// </summary>
public class TeamCatchSequence : MonoBehaviourPun
{
    /// <summary>
    /// Starts a coroutine of the catch sequence
    /// <param name="playerObj">Players that attempted the catch</param>
    /// <param name="monster">Potential monster to catch (null in case of a miss)</param>
    /// </summary>
    public void CatchSuccess(List<Player> players, Monster monster)
    {
        if (players.Count < 2) return;
        if (monster)
        {
            StartCoroutine(Catch(players, monster));
        }
        else
        {
            StartCoroutine(Miss(players));
        }

        Debug.Log("Success");
    }

    public void CatchFailed(List<Player> players)
    {
        if (players.Count < 2) return;
        StartCoroutine(Miss(players));
        Debug.Log("Failed");
    }

    private IEnumerator Miss(List<Player> players)
    {
        yield return null;
    }

    private IEnumerator Catch(List<Player> players, Monster monster)
    {
        PhotonNetwork.Instantiate(
                                  "Prefabs/KillParticles",
                                  monster.transform.position,
                                  Quaternion.identity,
                                  0,
                                  new object[] {monster.Color.id});
        monster.gameObject.SetActive(false);
        monster.photonView.RPC("KillMonsterRPC", RpcTarget.MasterClient, monster.photonView.ViewID);
        yield return null;
    }
}