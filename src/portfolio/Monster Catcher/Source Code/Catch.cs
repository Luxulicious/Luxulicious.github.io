using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Thrown when a catch attempt was made
/// Params: Game object of player; Game object of monster hit
/// </summary>
[Serializable]
public class OnCatchAttemptEvent : UnityEvent<GameObject, GameObject> { }


/// <summary>
/// Thrown for catch attempt revolving around audio
/// Param: Success/Failed
/// </summary>
[Serializable]
public class OnPlayCatchAudio : UnityEvent<bool> { }

/// <summary>
/// Use for catching monsters.
/// Invokes various events for the state of the catch (succeeded, missed etc...).
/// </summary>
public class Catch : MonoBehaviourPun
{
    [Tooltip("Player that should drop the trap. Can be left empty in case of teams")]
    public Player player;
    public BoxCollider2D boxCollider;
    [Space]
    public OnCatchAttemptEvent onCatchAttemptEvent = new OnCatchAttemptEvent();
    public OnCatchFailedEvent onCatchFailedEvent = new OnCatchFailedEvent();
    public OnCatchSuccessEvent onCatchSuccessEvent = new OnCatchSuccessEvent();
    public OnPlayCatchAudio onPlayCatchAudioEvent = new OnPlayCatchAudio();


    private string _playerColorId = "";
    private Rigidbody2D _rb;

    void Start()
    {
        if (!player)
            player = this.gameObject.GetComponent<Player>();

        _rb = GetComponent<Rigidbody2D>();
        if (!boxCollider)
            this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Drops trap for the players
    /// </summary>
    /// <param name="players">Players that drop the trap (can be 1)</param>
    public void DropTrapWithPlayers(List<Player> players)
    {
        if (!photonView.IsMine)
            return;

        if (_playerColorId.Length <= 0)
            _playerColorId = this.GetComponentInChildren<ColorRenderer>().GetColor().id;

        Monster monster = null;
        foreach (Collider2D obj in Physics2D.OverlapAreaAll(boxCollider.bounds.min, boxCollider.bounds.max))
        {
            var monsterCatchArea = obj.gameObject.GetComponent<MonsterCatchArea>();
            if (monsterCatchArea &&
                obj.transform.parent.GetComponent<ColorRenderer>().GetColor().id == _playerColorId)
            {
                monster = monsterCatchArea.Monster;
                break;
            }
        }
        players.ForEach(x => Debug.Log(x.GetInstanceID()));

        onPlayCatchAudioEvent.Invoke(monster != null);
        if (monster)
        {
            onCatchAttemptEvent.Invoke(this.gameObject, monster.gameObject);
            onCatchSuccessEvent.Invoke(players, monster);
        }
        else
        {
            onCatchAttemptEvent.Invoke(this.gameObject, null);
            onCatchFailedEvent.Invoke(players);
        }
    }

    /// <summary>
    /// Simplified way of invoking DropTrapWithPlayers(List<Player> players) with only this player
    /// </summary>
    public void DropTrap()
    {
        DropTrapWithPlayers(new List<Player>() {player});
    }
}