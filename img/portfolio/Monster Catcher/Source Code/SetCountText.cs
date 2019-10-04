using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for setting a number into a text element
/// </summary>
public class SetCountText : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    string _format = "{0} x";

    public void SetCount(int count)
    {
        _text.text = String.Format(_format, count);
    }
}
