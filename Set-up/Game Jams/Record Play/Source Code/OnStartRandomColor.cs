using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnStartRandomColor : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    public SpriteRenderer sp;

    // Use this for initialization
    void Start()
    {
        if (colors.Any())
            sp.color = colors[Random.Range(0, colors.Count)];
    }

}
