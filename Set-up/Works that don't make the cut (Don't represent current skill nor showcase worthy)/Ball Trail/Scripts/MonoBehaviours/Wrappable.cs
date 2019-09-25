using System;
using UnityEngine;

[Serializable]
public class Wrappable : MonoBehaviour
{
    public Collider2D col;
    public Wrappable clone;
    public bool escaping;

    public Wrappable(Collider2D col)
    {
        this.col = col;
        this.clone = null;
        this.escaping = false;
    }
}