using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///Used for setting a time signature in a text element
/// </summary>
public class SetTimeText : MonoBehaviour
{
    public Text text;

    void Start()
    {
        if (!text)
            text.GetComponent<Text>();
    }

    public void SetTime(float t)
    {
        var seconds = TimeSpan.FromSeconds(t).Seconds;
        text.text = seconds.ToString();
    }

    public void EmphasizeText()
    {
        text.color = Color.red;
        gameObject.AddComponent<GrowAndShrink>();
    }
}