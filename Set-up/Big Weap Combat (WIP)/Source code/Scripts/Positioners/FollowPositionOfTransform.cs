using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositionOfTransform : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _target.position;
    }
}
