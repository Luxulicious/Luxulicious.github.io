using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 50f;

    public void Update()
    {

        this.transform.Rotate(-new Vector3(0, 0, 1) * Time.deltaTime * speed);
    }

}
