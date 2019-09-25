using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//[Serializable]
//public class OnPartiallyOutOfRectangleEvent : UnityEvent<Collider2D> { }
//[Serializable]
//public class OnCompletelyOutOfRectangleEvent : UnityEvent<Collider2D> { }

public class InRectangle : MonoBehaviour
{
    public List<Collider2D> myCols = new List<Collider2D>();
    public Rectangle rectangle;
    //public OnCompletelyOutOfRectangleEvent onCompletelyOutOfRectangleEvent = new OnCompletelyOutOfRectangleEvent();
    //public OnPartiallyOutOfRectangleEvent onPartiallyOutOfRectangleEvent = new OnPartiallyOutOfRectangleEvent();

    void Start()
    {
        if (!myCols.Any())
            myCols = this.GetComponentsInChildren<Collider2D>().ToList();
    }

    void Update()
    {
        foreach (var myCol in myCols)
        {
            var boundingBox = myCol as BoxCollider2D;
            if (boundingBox != null)
            {
                var myColPosition = (Vector2)boundingBox.transform.position;
                var myColPointA = myColPosition + new Vector2(-boundingBox.bounds.extents.x, -boundingBox.bounds.extents.y);
                var myColPointB = myColPosition + new Vector2(boundingBox.bounds.extents.x, -boundingBox.bounds.extents.y);
                var myColPointC = myColPosition + new Vector2(boundingBox.bounds.extents.x, boundingBox.bounds.extents.y);
                var myColPointD = myColPosition + new Vector2(-boundingBox.bounds.extents.x, boundingBox.bounds.extents.y);
                var rectSize = rectangle.GetSize();
                var rectBottomLeft = rectangle.GetBottomLeft();
                var rectTopRight = rectangle.GetTopRight();
                if (myCol.transform.position.x - myCol.bounds.extents.x < rectBottomLeft.x)
                {
                    //Out left;
                }
                else if (myCol.transform.position.x + myCol.bounds.extents.x > rectTopRight.x)
                {
                    //Out right
                }
                else if (myCol.transform.position.x - myCol.bounds.extents.x >= rectBottomLeft.x)
                {
                    //In left
                }

                else if (false)
                {
                    //TODO Check for Y
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
