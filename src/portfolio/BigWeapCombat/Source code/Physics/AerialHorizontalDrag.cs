using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartData.SmartBool;
using UnityEngine;

public class AerialHorizontalDrag : HorizontalDrag
{
    [SerializeField] private BoolReader _grounded;

    new void FixedUpdate()
    {
        if (!_grounded.value)
            base.FixedUpdate();
    }
}