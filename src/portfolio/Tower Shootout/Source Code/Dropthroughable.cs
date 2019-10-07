using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Util;


public class Dropthroughable : MonoBehaviour
{
    [SerializeField] private List<Collider2D> _colliders;
    [SerializeField, ReadOnly] private List<Dropper> _droppersToIgnore;

    public List<Collider2D> Colliders
    {
        get { return _colliders; }
    }

    void OnEnable()
    {
        var liveDroppers = Dropper.GetLiveDroppers();
        if (!liveDroppers.Any())
            Debug.LogWarning("No live live droppers detected!");

        //Listen to dropper events
        liveDroppers.ForEach(x => { AddListenerToDropEvents(x); });

        //Set-up for live dropper changes
        Dropper.onRemoveLiveDropperEvent.AddListener(
            OnRemoveLiveDropper);
        Dropper.onAddedLiveDropperEvent.AddListener(
            OnAddedLiveDropper);
    }

    void OnDisable()
    {
        var liveDroppers = Dropper.GetLiveDroppers();
        if (!liveDroppers.Any())
        {
            Debug.LogWarning("No live live droppers detected!");
            return;
        }

        //Listen to dropper events
        liveDroppers.ForEach(x => { RemoveListenerFromDropEvents(x); });
    }


    private void OnRemoveLiveDropper(Dropper dropper)
    {
        //Remove listener when dropper left the scene
        RemoveListenerFromDropEvents(dropper);
    }

    private void OnAddedLiveDropper(Dropper dropper)
    {
        //Add listener when dropper came into the scene
        AddListenerToDropEvents(dropper);
    }

    private void RemoveListenerFromDropEvents(Dropper dropper)
    {
        dropper.RemoveListenerOnDropEnterEvent(OnDropEnterEventHandler);
        dropper.RemoveListenerOnDropStayEvent(OnDropStayEventHandler);
        dropper.RemoveListenerOnDropExitEvent(OnDropExitEventHandler);
    }

    private void AddListenerToDropEvents(Dropper dropper)
    {
        dropper.AddListenerOnDropEnterEvent(OnDropEnterEventHandler);
        dropper.AddListenerOnDropExitEvent(OnDropExitEventHandler);
        dropper.AddListenerOnDropStayEvent(OnDropStayEventHandler);
    }

    #region Drop event processing

    private void OnDropEnterEventHandler(Dropper dropper)
    {
        DisableCollisionsWithDropper(dropper);
    }


    private void OnDropStayEventHandler(Dropper dropper)
    {
        DisableCollisionsWithDropper(dropper);
    }

    private void OnDropExitEventHandler(Dropper dropper)
    {
        StartCoroutine(OnDropExitCoroutine(dropper));
    }

    public IEnumerator OnDropExitCoroutine(Dropper dropper)
    {
        while (DropperOverlapsColliders(dropper, this._colliders))
        {
            yield return new WaitForFixedUpdate();
        }
        EnableCollisionsWithDropper(dropper);
        yield return null;
    }

    private bool DropperOverlapsColliders(Dropper dropper, List<Collider2D> area)
    {
        var overlaps = false;
        foreach (var x in dropper.Colliders)
        {
            foreach (var y in _colliders)
            {
                //TODO This maybe will only work for axis aligned colliders,
                //which may not be desirable.
                //Account for non-axis-aligned colliders as well then!
                if (x.bounds.Intersects(y.bounds))
                {
                    overlaps = true;
                    break;
                }
            }
        }
        return overlaps;
    }

    private void EnableCollisionsWithDropper(Dropper dropper)
    {
        if (!_droppersToIgnore.Contains(dropper)) return;
        dropper.Colliders.ForEach(x => { this._colliders.ForEach(y => { Physics2D.IgnoreCollision(x, y, false); }); });
        _droppersToIgnore.Remove(dropper);
    }


    private void DisableCollisionsWithDropper(Dropper dropper)
    {
        if (_droppersToIgnore.Contains(dropper)) return;
        _droppersToIgnore.Add(dropper);
        dropper.Colliders.ForEach(x => { this._colliders.ForEach(y => { Physics2D.IgnoreCollision(x, y, true); }); });
    }

    #endregion
}