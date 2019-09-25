using System;
using System.Collections.Generic;
using System.Linq;
using SmartData.SmartBool.Data;
using UnityEngine;

public class ScalerCollisionResolver : MonoBehaviour
{
    public delegate Vector2 RayOriginDelegate(int i, Vector2 pos, Vector2 extents, float rayPadding);

    [Serializable]
    public struct PrecisionSettings
    {
        [Range(2, 1000)] public int _rayPerEdge;
    }

    //TODO Decouple this via interface or event
    [SerializeField] private CollisionCollisionTypeDetection _collisionCollisionTypeDection;
    [SerializeField] private Transform _bodyToDislocate;
    [SerializeField] private Collider2D _bodyToDislocateCollider;
    [SerializeField] private Transform _pivot;
    [SerializeField] private PrecisionSettings _precisionSettings;

    //TODO Make this auto determine on start-up
    [SerializeField, LabelOverride("Original size")]
    private Vector2 _oSize = new Vector2(1, 3);

    void Awake()
    {
        if (!_collisionCollisionTypeDection)
            _collisionCollisionTypeDection = this.GetComponent<CollisionCollisionTypeDetection>();
        if (!_pivot)
            throw new Exception("No pivot point set!");
        if (!_bodyToDislocate)
            throw new Exception("No body to dislocate set!");
        else if (!_bodyToDislocateCollider)
            _bodyToDislocate.GetComponent<Collider2D>();
    }

    public void ResolveSizeUp(ScaleState scaleState)
    {
        ResolveSizeUp(scaleState.prevScale, scaleState.currentScale);
    }

    public void ResolveSizeUp(Vector2 prevSize, Vector2 currentScale)
    {
        //Check if actually grown
        if (!(prevSize.x < currentScale.x) && !(prevSize.y < currentScale.y)) return;

        //Calculate difference in size
        var deltaSize = (currentScale - prevSize) * _oSize;
        //Determine orientation via pivot points
        var dirY = _pivot.up;
        var dirX = _pivot.right;
        //Determine expected position difference
        var deltaPos = -dirX * deltaSize.x / 2 + -dirY * deltaSize.y;


        if (_collisionCollisionTypeDection.GetActiveCollisions().dynamicCollisions.Any())
        {
            throw new NotImplementedException();
        }

        if (_collisionCollisionTypeDection.GetActiveCollisions().immovableCollisions.Any())
        {
            LayerMask immovableLayerMask = _collisionCollisionTypeDection.GetCollisionTypes().GetLayerMaskImmovable();
            var offset = deltaPos;
            var hits = RaycastHit2Ds(dirY, immovableLayerMask, offset);

            //If the length of a ray was less then the expected dislocation
            hits.RemoveAll(x => (x.distance - offset.magnitude) < 0);

            hits.ForEach(x =>
            {
                Debug.DrawLine(x.point, x.point + new Vector2(0.1f, 0.1f), Color.green);
                Debug.DrawLine(x.point, x.point + new Vector2(-0.1f, -0.1f), Color.green);
                Debug.DrawLine(x.point, x.point + new Vector2(0.1f, -0.1f), Color.green);
                Debug.DrawLine(x.point, x.point + new Vector2(-0.1f, 0.1f), Color.green);
            });

            var deltaPosTooLong = hits.Any(x => (x.distance - offset.magnitude) < deltaPos.magnitude);

            if (deltaPosTooLong)
            {
                //TODO Repeat the process as long as possible till the length of
                //the thing would not increase when dislocating
                //if cant dislocate further stop growth

                //Move the body as close to the shortest ray hit as possible
                var shortestHit = hits.OrderBy(x => x.distance).First();
                var shortestDistance = shortestHit.distance;
                _bodyToDislocate.position = shortestHit.point;
                //TODO Body partially can get into wall fix this by moving the body away
                if (shortestHit.normal == Vector2.up || shortestHit.normal == Vector2.down || shortestHit.normal == Vector2.right || shortestHit.normal == Vector2.left)
                {
                    _bodyToDislocate.position += new Vector3(
                        shortestHit.normal.x * _bodyToDislocateCollider.bounds.extents.x,
                        shortestHit.normal.y * _bodyToDislocateCollider.bounds.extents.y);
                }
                else
                {
                    Debug.LogError("Can't resolve collision of surface normal: " + shortestHit.normal);
                    //TODO Resolve collision differently
                }

                //Add the remainder length of the delta position perpendicular to the normal of the ray cast hit
                var clingDistance = deltaPos.magnitude - shortestDistance;
                //Compare the two possible vectors via dot product to the direction currently moving in
                var clingVectorOptionA = clingDistance * new Vector2(-shortestHit.normal.y, shortestHit.normal.x);
                var clingVectorOptionB = clingDistance * new Vector2(shortestHit.normal.y, -shortestHit.normal.x);
                var dotOptionA = Vector2.Dot(clingVectorOptionA, -dirY);
                var dotOptionB = Vector2.Dot(clingVectorOptionB, -dirY);
                Vector2 additiveVector;
                if (dotOptionA > dotOptionB)
                    additiveVector = clingVectorOptionA;
                else if (dotOptionB > dotOptionA)
                    additiveVector = clingVectorOptionB;
                else
                    throw new NotImplementedException("Could not determine dislocation further...");

                //Shoot another ray parallel to the additive vector2 to see if it needs to be shortened
                var newHit = Physics2D.Raycast(shortestHit.point, additiveVector.normalized, Mathf.Infinity,
                    immovableLayerMask);
                if (newHit.collider != null)
                {
                    //TODO Add recursion to always allow for more growth
                    if (newHit.distance < additiveVector.magnitude)
                    {
                        additiveVector = additiveVector.normalized * newHit.distance;
                    }
                }

                //Dislocate the body
                _bodyToDislocate.position += (Vector3) additiveVector;
                //Prevent the body from clipping by dislocating the
                //body relative to the surface normal "times" the delta position or delta size
                _bodyToDislocate.position -=
                    new Vector3(shortestHit.normal.x * deltaPos.x, shortestHit.normal.y * deltaPos.y);


                //Draw lines indicating what happened
                Debug.DrawLine(shortestHit.point, shortestHit.point + new Vector2(0.05f, 0.05f), Color.magenta);
                Debug.DrawLine(shortestHit.point, shortestHit.point + new Vector2(-0.05f, 0.05f), Color.magenta);
                Debug.DrawLine(shortestHit.point, shortestHit.point + new Vector2(0.05f, -0.05f), Color.magenta);
                Debug.DrawLine(shortestHit.point, shortestHit.point + new Vector2(-0.05f, -0.05f), Color.magenta);
                Debug.DrawLine(shortestHit.point, shortestHit.point + additiveVector, Color.magenta);
            }
            else
            {
                //Dislocate the body relative to the scale-up 
                var oPos = _bodyToDislocate.position;
                var nPos = oPos + deltaPos;
                Debug.DrawLine(oPos, nPos, Color.red);
                _bodyToDislocate.position = nPos;
            }
        }
    }

    private List<RaycastHit2D> RaycastHit2Ds(Vector3 dirY, LayerMask layerMask, Vector2 offset)
    {
        //Validate dislocation
        var pos = _bodyToDislocate.position - (Vector3) offset;
        var size = _bodyToDislocateCollider.bounds.size;
        var rayPaddings = new Vector2(
            size.x / (_precisionSettings._rayPerEdge - 1),
            size.y / (_precisionSettings._rayPerEdge - 1)
        );
        var extents = size / 2;
        var dir = -dirY;

        var hits = new List<RaycastHit2D>();
        if (dir.x < 0)
            hits.AddRange(CastEdgeRays(pos, extents, rayPaddings.y, dir, layerMask,
                RayOriginLeftEdge));
        else if (dir.x > 0)
            hits.AddRange(CastEdgeRays(pos, extents, rayPaddings.y, dir, layerMask,
                RayOriginRightEdge));
        if (dir.y > 0)
            hits.AddRange(CastEdgeRays(pos, extents, rayPaddings.x, dir, layerMask,
                RayOriginTopEdge));
        else if (dir.y < 0)
            hits.AddRange(CastEdgeRays(pos, extents, rayPaddings.x, dir, layerMask,
                RayOriginBottomEdge));
        return hits;
    }

    public List<RaycastHit2D> CastEdgeRays(Vector2 pos, Vector2 extents, float rayPadding,
        Vector2 dir, LayerMask immovableLayerMask, RayOriginDelegate rayOriginDelegate)
    {
        var hits = new List<RaycastHit2D>();
        for (int i = 0; i < _precisionSettings._rayPerEdge; i++)
        {
            var origin = rayOriginDelegate(i, pos, extents, rayPadding);
            var hit = Physics2D.Raycast(origin, dir, Mathf.Infinity, immovableLayerMask);
            if (hit.collider != null)
            {
                Debug.DrawLine(origin, origin + new Vector2(0.1f, 0.1f), Color.red);
                Debug.DrawLine(origin, origin + new Vector2(-0.1f, -0.1f), Color.red);
                Debug.DrawLine(origin, origin + new Vector2(0.1f, -0.1f), Color.red);
                Debug.DrawLine(origin, origin + new Vector2(-0.1f, 0.1f), Color.red);
                Debug.DrawLine(origin, origin + dir * hit.distance, Color.red);
                hits.Add(hit);
            }
        }

        return hits;
    }

    public Vector2 RayOriginLeftEdge(int i, Vector2 pos, Vector2 extents, float heightRayPadding)
    {
        return pos - new Vector2(extents.x, extents.y) +
               new Vector2(0, heightRayPadding) * i;
    }

    public Vector2 RayOriginRightEdge(int i, Vector2 pos, Vector2 extents, float heightRayPadding)
    {
        return pos - new Vector2(-extents.x, extents.y) +
               new Vector2(0, heightRayPadding) * i;
    }

    public Vector2 RayOriginTopEdge(int i, Vector2 pos, Vector2 extents, float widthRayPadding)
    {
        return pos - new Vector2(extents.x, -extents.y) +
               new Vector2(widthRayPadding, 0) * i;
    }

    public Vector2 RayOriginBottomEdge(int i, Vector2 pos, Vector2 extents, float widthRayPadding)
    {
        return pos - new Vector2(extents.x, extents.y) +
               new Vector2(widthRayPadding, 0) * i;
    }
}