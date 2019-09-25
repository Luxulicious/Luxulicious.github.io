using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void DestroySafe(this GameObject go)
    {
        OnBeforeDisable(go);
        GameObject.Destroy(go);
    }

    public static void SetActiveSafe(this GameObject go, bool active)
    {
        if (!active && go.activeSelf)
        {
            OnBeforeDisable(go);
        }

        go.SetActive(active);
    }

    //TODO Find a way to always call this with OnDestroy/Disable
    private static void OnBeforeDisable(GameObject go)
    {
        var disablable = go.GetComponent<IDisablable>();
        if (disablable != null) disablable.OnBeforeDisable();
        go.GetComponentsInChildren<IDisablable>().ToList().ForEach(x => x.OnBeforeDisable());
    }
}