using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO Change to pair of points
public class Rectangle : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _points = new List<Transform>();

    void Update()
    {
        //TODO Validation
    }

    public Vector2 GetSize()
    {
        var lowest = GetBottomLeft();
        var highest = GetTopRight();
        var size = highest - lowest;
        return size;
    }

    public Vector2 GetBottomLeft()
    {
        var val = new List<Transform>();
        val.AddRange(this._points);
        return val.OrderBy(x => x.position.y + x.position.x).First().position;
    }

    public Vector2 GetTopRight()
    {
        var val = new List<Transform>();
        val.AddRange(this._points);
        return val.OrderByDescending(x => x.position.y + x.position.x).First().position;
    }

    public List<Vector2> GetPoints()
    {
        var val = new List<Vector2>();
        _points.ForEach(x => val.Add(x.position));
        return val;
    }
}
