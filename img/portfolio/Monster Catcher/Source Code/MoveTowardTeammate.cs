using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Util;

[Serializable]
public class OnConnectedWithTeammateEvent : UnityEvent<List<Player>> { }

[Serializable]
public class OnConnectedWithNonTeammateEvent : UnityEvent { }

/// <summary>
/// Used for making teammates hone in each other to catch together
/// </summary>
public class MoveTowardTeammate : MonoBehaviourPun
{
    public Player myPlayer;
    public Rigidbody2D myRb;
    public float acceleration = 1f;
    [Tooltip("If selected, both teammates will move regardless of the other teammate's actions")]
    public bool moveSelfIfTeammateMovesToward = true;

    [ReadOnly]
    public bool movingTowardTeammate;

    [SerializeField, ReadOnly]
    private float speed = 0f;

    [SerializeField, ReadOnly]
    private List<Player> _teammates;

    [SerializeField, ReadOnly]
    private Player _targetTeammate;

    [SerializeField, ReadOnly]
    private MoveTowardTeammate _moveTowardTeammateTarget;

    [Space]
    public OnConnectedWithTeammateEvent onConnectedWithTeammateEvent = new OnConnectedWithTeammateEvent();
    public OnConnectedWithNonTeammateEvent onConnectedWithNonTeammateEvent = new OnConnectedWithNonTeammateEvent();

    void Start()
    {
        if (!myPlayer)
            myPlayer = this.GetComponent<Player>();
        if (!myRb)
            myRb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (movingTowardTeammate)
        {
            if (_targetTeammate)
            {
                speed += acceleration;
                this.myRb.velocity = (_targetTeammate.transform.position - this.transform.position) * speed *
                                     Time.deltaTime;
            }
        }
    }

    [PunRPC]
    private void StartMovingTowardEachOtherRPC()
    {
        StartMovingTowardEachOther(true);
    }

    public void StartMovingTowardEachOther()
    {
       StartMovingTowardEachOther(false);
    }

    /// <summary>
    /// Moves teammates toward each other
    /// </summary>
    /// <param name="isRPCCall">If already called from RPC, it does not need to send another RPC back, hence this param</param>
    private void StartMovingTowardEachOther(bool isRPCCall)
    {
        if (movingTowardTeammate) return;
        _teammates = Player.GetLiveTeamPlayers(myPlayer);
        if (_teammates.Count == 1)
        {
            //Start moving
            _targetTeammate = _teammates.First();
            movingTowardTeammate = true;

            if (isRPCCall) return;

            //Check if other player should move along as well
            _moveTowardTeammateTarget = _targetTeammate.GetComponent<MoveTowardTeammate>();
            if (!_moveTowardTeammateTarget) return;
            if (!_moveTowardTeammateTarget.moveSelfIfTeammateMovesToward) return;

            _moveTowardTeammateTarget.photonView.RPC("StartMovingTowardEachOtherRPC", _moveTowardTeammateTarget.photonView.Owner);
        }
        else
        {
            movingTowardTeammate = false;
            throw new NotImplementedException(typeof(MoveTowardTeammate).ToString() + "::" +
                                              "StartMovingTowardEachOther -->" + "Could not handle " +
                                              _teammates.Count + " amount of teammates.");
        }
    }

    public void StopMovingTowardEachother()
    {
        movingTowardTeammate = false;
        this.myRb.velocity = Vector2.zero;
        speed = 0;

        if (_targetTeammate)
        {
            if (_moveTowardTeammateTarget)
            {
                _moveTowardTeammateTarget._targetTeammate = null;
                _moveTowardTeammateTarget.StopMovingTowardEachother();
                if (_moveTowardTeammateTarget.myRb)
                    _moveTowardTeammateTarget.myRb.velocity = Vector2.zero;
                else
                {
                    //TODO Replace this with some other component
                    var rb = _targetTeammate.GetComponent<Rigidbody2D>();
                    if (rb)
                        rb.velocity = Vector2.zero;
                }
            }
            else
            {
                //TODO Replace this with some other component
                var rb = _targetTeammate.GetComponent<Rigidbody2D>();
                if (rb)
                    rb.velocity = Vector2.zero;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        var otherPlayer = col.collider.GetComponent<Player>();
        if (!otherPlayer) return;
        if (!movingTowardTeammate) return;
        if (!_targetTeammate) return;
        if (otherPlayer.photonView.ViewID == _targetTeammate.photonView.ViewID)
        {
            onConnectedWithTeammateEvent.Invoke(new List<Player>() {myPlayer, otherPlayer});
        }
        else
        {
            onConnectedWithNonTeammateEvent.Invoke();
        }
        
    }
}