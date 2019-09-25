using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextUI : MonoBehaviour
{
    public Text textUIElement;

    public void SetText(string text)
    {
        textUIElement.text = text;
    }
}
