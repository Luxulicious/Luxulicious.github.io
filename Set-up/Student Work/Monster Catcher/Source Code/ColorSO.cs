using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Scriptable object for colors in the game
/// </summary>
[CreateAssetMenu(fileName = "Color", menuName = "Color", order = 1)]
public class ColorSO : ScriptableObject
{
    [Tooltip("GUID; Used in comparisons if ColorSOs are equal.")]
    public string id;
    [Tooltip("Color that will be rendered from the ColorSO. Not recommended to use for equal comparisons.")]
    public Color color;

    /// <summary>
    /// Generates a new Id 
    /// </summary>
    [ContextMenu("Generate new Id")]
    void GenerateNewId()
    {
        id = Guid.NewGuid().ToString();
    }
}
