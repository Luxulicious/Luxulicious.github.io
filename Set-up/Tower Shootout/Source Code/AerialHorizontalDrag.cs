using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AerialHorizontalDrag : HorizontalDrag
{
    [SerializeField]
    private GroundedState _groundedState;

    new void FixedUpdate()
    {
        if (!_groundedState.IsGrounded())
            base.FixedUpdate();
    }
}