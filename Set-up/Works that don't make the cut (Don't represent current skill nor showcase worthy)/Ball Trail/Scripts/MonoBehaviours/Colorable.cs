using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnCollisionEnterSameColorableEvent : UnityEvent<Collision2D> { }
[Serializable]
public class OnCollisionEnterOtherColorableEvent : UnityEvent<Collision2D> { }

[Serializable]
public class Colorable : MonoBehaviour
{
    //Determines wether colorables are actually "same"
    public SpriteRenderer sp;
    public ColorSO color;

    public OnCollisionEnterSameColorableEvent onCollisionEnterSameColorableEvent = new OnCollisionEnterSameColorableEvent();
    public OnCollisionEnterOtherColorableEvent onCollisionEnterOtherColorableEvent = new OnCollisionEnterOtherColorableEvent();

    protected void Start()
    {
        if (!sp)
            sp = this.GetComponent<SpriteRenderer>();
        var availableColorable = AvailableColors.instance.GetColorById(color.collisionId);
        if (!availableColorable)
            Debug.LogError("Collision id not allowed.");
        else
        {
            if (availableColorable != this.color)
                Debug.LogWarning("Available color is not the same as " + this.name + "'s color");
        }
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        var colColorable = col.collider.GetComponent<Colorable>();
        if (colColorable)
        {
            if (this.color.collisionId == colColorable.color.collisionId)
                onCollisionEnterSameColorableEvent.Invoke(col);
            else
                onCollisionEnterOtherColorableEvent.Invoke(col);
        }
    }

    public void SetColorable(ColorSO color)
    {
        this.color = color;
        if (!sp)
            sp = this.GetComponent<SpriteRenderer>();
        if (sp)
            sp.color = color.color;

    }
}
