using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSettings : MonoBehaviour
{
    public Camera camera;
    public float aspect = 1f;

    void Update()
    {
        camera.aspect = aspect;
    }
}
