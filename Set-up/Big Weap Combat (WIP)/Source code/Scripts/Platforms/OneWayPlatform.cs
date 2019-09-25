using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private List<Collision2D> _disabledCollisions = new List<Collision2D>();

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<OnPlatformTrigger>())
        {
            foreach (var contact in col.contacts)
            {
                if (IsBottomCollision(contact))
                {
                    this._disabledCollisions.Add(col);
                    Physics2D.IgnoreCollision(col.collider, col.otherCollider, true);
                    var platformTriggerExitHandler = col.gameObject.GetComponent<OnPlatformTrigger>();
                    platformTriggerExitHandler.platformExitEvent.AddListener(OnPlatformTriggerExit);
                }
            }
        }
    }

    /// <summary>
    /// Determines whether or not a point of contact was at the bottom of the platform.
    /// </summary>
    /// <param name="contact">Contactpoint</param>
    /// <returns></returns>
    private static bool IsBottomCollision(ContactPoint2D contact)
    {
        //TODO Maybe change to degrees instead
        //TODO Add sides and corners to this equation as well
        return (Mathf.Round(contact.normal.x * 100) / 100 >= -1.0f ||
                Mathf.Round(contact.normal.x * 100) / 100 <= 1.0f)
               && Mathf.Round(contact.normal.y * 100) / 100 >= 0.0f;
    }

    /// <summary>
    /// Re-enables a collision
    /// </summary>
    /// <param name="col">Collider that should collide again with this collider</param>
    void OnPlatformTriggerExit(Collider2D col)
    {
        var toRemoveList = new List<Collision2D>();
        foreach (var disabledCollision in _disabledCollisions)
        {
            var platformCol = disabledCollision.otherCollider;
            var physicalObjCol = disabledCollision.collider;
            if (col.gameObject.Equals(physicalObjCol.gameObject))
            {
                Physics2D.IgnoreCollision(platformCol, physicalObjCol, false);
                var onPlatformTrigger = col.gameObject.GetComponent<OnPlatformTrigger>();
                if (onPlatformTrigger == null)
                    onPlatformTrigger = col.GetComponentInParent<OnPlatformTrigger>();
                onPlatformTrigger.platformExitEvent.RemoveListener(OnPlatformTriggerExit);
                toRemoveList.Add(disabledCollision);
            }
        }
        foreach (var toRemove in toRemoveList)
        {
            var disabledCollisionsCount = _disabledCollisions.Count;
            if (!this._disabledCollisions.Remove(toRemove))
                Debug.LogWarning("Failed to remove a disabledCollider from platform.");
            if (this._disabledCollisions.Count >= disabledCollisionsCount)
                Debug.LogWarning("Failed to remove a disabledCollider from platform.");
        }
    }

}

