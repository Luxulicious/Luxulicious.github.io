using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ICollisionTypeDetection
{
    bool IsCollidingWithImmovable();
    bool IsCollidingWithDynamic();
}