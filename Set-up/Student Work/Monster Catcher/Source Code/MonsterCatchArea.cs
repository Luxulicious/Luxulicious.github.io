using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effectively a tag for defining the hurt box of a monster
/// </summary>
public class MonsterCatchArea : MonoBehaviour
{
    [SerializeField]
    private Monster _monster;

    void Start()
    {
        if (!_monster)
        {
            _monster = this.GetComponent<Monster>();
        }
        if(!_monster)
        {
            _monster = this.transform.parent.GetComponent<Monster>();
        }
    }

    public Monster Monster
    {
        get { return _monster; }
    }
}
