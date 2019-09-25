using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Replace this with a less limited identification system

/// <summary>
/// Used for determining which entities in the scene belong to a certain "team"...
/// Functionally identical to tags.
/// </summary>
public enum CombattantType
{
    Enemy, Player, Default
}

/// <summary>
/// Determines what "team" an entity is on
/// </summary>
public class Combattant : MonoBehaviour
{
    public CombattantType combattantType = CombattantType.Default;

    void Start()
    {
        if (this.combattantType == CombattantType.Default)
        {
            Debug.LogWarning("Combattant type of " + this.name + " is set to default. Setting a combattant type beforehand may be required!");
        }
    }
}
