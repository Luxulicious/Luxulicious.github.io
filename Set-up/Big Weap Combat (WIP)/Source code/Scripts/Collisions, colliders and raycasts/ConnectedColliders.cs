using System.Collections.Generic;
using UnityEngine;

public class ConnectedColliders : MonoBehaviour
{
    [SerializeField]
    private List<Collider2D> _colliders = new List<Collider2D>();

    public List<Collider2D> Colliders
    {
        get { return _colliders; }
    }
}