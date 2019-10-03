using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextUI : MonoBehaviour
{
    [SerializeField]
    private Text _unityText;

    public void SetText(string text)
    {
        _unityText.text = text;
    }

    public void SetText(int integer)
    {
        _unityText.text = integer.ToString();
    }
}
