using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Color", menuName = "Color")]
public class ColorSO : ScriptableObject
{
    public Color color;
    public int collisionId;
}
