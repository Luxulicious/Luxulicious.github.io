using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public Transform subject;

    void Awake()
    {
        if (!this.subject)
            this.subject = this.transform;
    }

    public void SetGameObjectActiveSafe()
    {
        subject.gameObject.SetActiveSafe(true);
    }

    public void SetGameObjectActiveSafe(bool active)
    {
        subject.gameObject.SetActiveSafe(active);
    }

    public void SetGameObjectInactiveSafe()
    {
        subject.gameObject.SetActiveSafe(false);
    }
}
