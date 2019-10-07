using System;
using UnityEngine;

/// <summary>
/// Contains player data for the game set-up to instantiate
/// </summary>
[Serializable]
public struct PlayerData
{
    public ColorSO color;
    public GameObject prefab;
    public Transform spawnPosition;
    public Transform cameraPosition;
}