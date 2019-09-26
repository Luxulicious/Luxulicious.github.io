using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public enum PathType
{
    loop,
    linear
}

//TODO Refactor the overlap between move fixed update and update with a delegate
public class MoveAlongPath : MonoBehaviour
{
    public bool moveOnEnable = true;

    [Tooltip("Makes the path loop in on itself or loop linearly.")]
    public PathType pathType = PathType.loop;

    public float speed = 5f;

    public Transform transformToMove;
    public bool keepMovingWhenDisabled = false;
    public bool keepMovingWhenCollidersDisabled = false;

    [SerializeField]
    private List<Collider2D> _cols = new List<Collider2D>();

    [Tooltip("Insert points here that indicate the path the object should be traveling")]
    public List<Transform> pathPoints;

    [SerializeField]
    private int _targetPathPointIndex = 0;

    public float positionComparisonThreshold = 0.25f;

    [SerializeField, ReadOnly]
    private bool invert;

    [Space, Header("Gizmos")]
    public bool showGizmos = true;

    public Color gizmoColor = Color.white;

    void OnEnable()
    {
        if (!transformToMove)
            transformToMove = this.transform;
        if (!_cols.Any())
            _cols.AddRange(transformToMove.GetComponents<Collider2D>());
        if (moveOnEnable)
            StartCoroutine(Move());
    }

    void OnDisable()
    {
        if (keepMovingWhenDisabled)
            StopCoroutine(Move());
    }

    public void StartMoving()
    {
        StopMoving();
        StartCoroutine(Move());
    }

    public void StopMoving()
    {
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (!keepMovingWhenDisabled && !transformToMove.gameObject.activeSelf)
        {
            yield return new WaitForEndOfFrame();
        }

        if (_cols.Any())
            while ((!_cols.TrueForAll(x => x.enabled) && !keepMovingWhenCollidersDisabled) ||
                   !_cols.Exists(x => x.gameObject.activeSelf))
                yield return new WaitForEndOfFrame();
        while (pathPoints == null)
            yield return new WaitForEndOfFrame();
        while (!pathPoints.Any())
            yield return new WaitForEndOfFrame();
        while (true)
        {
            MoveUpdate();
            yield return new WaitForEndOfFrame();
        }
    }


    private void MoveUpdate()
    {
        var targetPosition = (Vector2) pathPoints[_targetPathPointIndex].position;
        if (new Vector2().Equals(transformToMove.position, targetPosition, positionComparisonThreshold))
        {
            transformToMove.position = targetPosition;
            if (pathType == PathType.loop)
            {
                if (_targetPathPointIndex < pathPoints.Count - 1)
                    _targetPathPointIndex++;
                else
                    _targetPathPointIndex = 0;
            }
            else
            {
                if (_targetPathPointIndex < pathPoints.Count - 1 && !invert)
                    _targetPathPointIndex++;
                else if (_targetPathPointIndex > 0 && invert)
                    _targetPathPointIndex--;
                else if (_targetPathPointIndex == pathPoints.Count - 1 && !invert)
                {
                    invert = true;
                    _targetPathPointIndex--;
                }
                else if (_targetPathPointIndex == 0 && invert)
                {
                    invert = false;
                    _targetPathPointIndex++;
                }
                else
                {
                    throw new Exception("Illegal target index: " + _targetPathPointIndex);
                }
            }

            targetPosition = (Vector2) pathPoints[_targetPathPointIndex].position;
        }

        var delta = (Vector3) targetPosition - transformToMove.position;
        var dir = delta.normalized;
        transformToMove.position += dir * speed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        if (!this.gameObject.activeSelf || !this.enabled) return;
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (i != pathPoints.Count - 1)
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            else if (pathType.Equals(PathType.loop))
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[0].position);
        }

        var targetPosition = pathPoints[_targetPathPointIndex].position;
        Gizmos.DrawLine(targetPosition, targetPosition + new Vector3(1, 1));
        Gizmos.DrawLine(targetPosition, targetPosition + new Vector3(-1, 1));
        Gizmos.DrawLine(targetPosition, targetPosition + new Vector3(-1, -1));
        Gizmos.DrawLine(targetPosition, targetPosition + new Vector3(1, -1));
    }
}