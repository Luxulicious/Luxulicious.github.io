using System;
using UnityEngine;

/// <summary>
/// Contains monster data for the game set-up to instantiate
/// </summary>
[Serializable]
public struct MonsterData
{
    public ColorSO color;
    public GameObject prefab;
    public Transform spawnPosition;
}