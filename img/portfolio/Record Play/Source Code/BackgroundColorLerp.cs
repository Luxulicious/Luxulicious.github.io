using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorLerp : MonoBehaviour
{
    public Camera camera;
    public List<Color> colors = new List<Color>();
    private Color lerpedColor;

    void Update()
    {
        StartCoroutine(ChangeColorAfterTime(5));
    }

    IEnumerator ChangeColorAfterTime(float delayTime)
    {
        Color currentcolor = (Color)colors[UnityEngine.Random.Range(0, colors.Count)]; ;
        Color nextcolor;

        camera.backgroundColor = currentcolor;

        while (true)
        {
            nextcolor = (Color)colors[UnityEngine.Random.Range(0, colors.Count)];

            for (float t = 0; t < delayTime; t += Time.deltaTime)
            {
                camera.backgroundColor = Color.Lerp(currentcolor, nextcolor, t / delayTime);
                yield return null;
            }
            currentcolor = nextcolor;
        }
    }
}
