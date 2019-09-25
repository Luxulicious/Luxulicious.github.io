using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//TODO Delegate left/right (un)cloning
//TODO Maybe do up/down as well(?)
//TODO Use instance pooling or ECS
public class WrapAround : MonoBehaviour
{
    public Rectangle wrappingBounds;
    public List<Wrappable> wrappables = new List<Wrappable>();

    void Start()
    {
        if (!wrappables.Any())
            this.wrappables = GetComponentsInChildren<Wrappable>().ToList();
        OrderWrappablesByPosition();
    }

    private void OrderWrappablesByPosition()
    {
        var orderedWrappables = from w in wrappables
                                orderby w.col.transform.position.x, w.col.transform.position.y
                                select w;
        wrappables = orderedWrappables.ToList();
    }

    void Update()
    {
        var mostRightWrappable = wrappables.Last();
        var mostRightCol = mostRightWrappable.col;
        if (mostRightCol.transform.position.x + mostRightCol.bounds.extents.x > wrappingBounds.GetTopRight().x)
        {
            CloneRight();
            if (mostRightCol.transform.position.x - mostRightCol.bounds.extents.x > wrappingBounds.GetTopRight().x)
            {
                UncloneRight();
            }
        }

        var mostLeftWrappable = wrappables.First();
        var mostLeftCol = mostLeftWrappable.col;
        if (mostLeftCol.transform.position.x - mostLeftCol.bounds.extents.x < wrappingBounds.GetBottomLeft().x)
        {
            CloneLeft();
            if (mostLeftCol.transform.position.x + mostLeftCol.bounds.extents.x < wrappingBounds.GetBottomLeft().x)
            {
                UncloneLeft();
            }
        }
    }

    private void UncloneLeft()
    {
        var mostLeftWrappable = wrappables.First();
        if (mostLeftWrappable.clone == null)
            return;
        if (mostLeftWrappable.clone.clone == null)
            return;
        mostLeftWrappable.clone.clone = null;
        mostLeftWrappable.clone = null;
        wrappables.Remove(mostLeftWrappable);
        Destroy(mostLeftWrappable.gameObject);
        OrderWrappablesByPosition();
    }

    private void UncloneRight()
    {
        var mostRightWrappable = wrappables.Last();
        if (mostRightWrappable.clone == null)
            return;
        if (mostRightWrappable.clone.clone == null)
            return;
        mostRightWrappable.clone.clone = null;
        mostRightWrappable.clone = null;
        wrappables.Remove(mostRightWrappable);
        Destroy(mostRightWrappable.gameObject);
        OrderWrappablesByPosition();
    }

    private void CloneLeft()
    {
        var mostLeftWrappable = wrappables.First();
        //Check if already cloned, if not continue
        if (mostLeftWrappable.clone != null)
            return;

        //Instantiate clone on the left
        var mostRightWrappable = wrappables.Last();
        var newMostRightWrappable = Instantiate(
            mostLeftWrappable,
            this.transform
        );
        newMostRightWrappable.col.transform.position = (Vector2)mostRightWrappable.col.transform.position +
                                                      (Vector2.right *
                                                       (mostLeftWrappable.col.bounds.extents.x +
                                                        mostRightWrappable.col.bounds.extents.x));
        newMostRightWrappable.name = mostLeftWrappable.name;

        //Set clone fields
        newMostRightWrappable.clone = mostLeftWrappable;
        wrappables.First().clone = newMostRightWrappable;

        //Add clone and resort
        wrappables.Add(newMostRightWrappable);
        OrderWrappablesByPosition();
    }

    private void CloneRight()
    {
        var mostRightWrappable = wrappables.Last();
        //Check if already cloned, if not continue
        if (mostRightWrappable.clone != null)
            return;

        //Instantiate clone on the left
        var mostLeftWrappable = wrappables.First();
        var newMostLeftWrappable = Instantiate(
            mostRightWrappable,
            this.transform
            );
        newMostLeftWrappable.col.transform.position = (Vector2)mostLeftWrappable.col.transform.position -
                                                      (Vector2.right *
                                                       (mostRightWrappable.col.bounds.extents.x +
                                                        mostLeftWrappable.col.bounds.extents.x));
        newMostLeftWrappable.name = mostRightWrappable.name;

        //Set clone fields
        newMostLeftWrappable.clone = mostRightWrappable;
        wrappables.Last().clone = newMostLeftWrappable;

        //Add clone and resort
        wrappables.Add(newMostLeftWrappable);
        OrderWrappablesByPosition();
    }
}