using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ConnectedRaycastHits2DQE : UnityEvent<RichRaycastHit2D[]> { }

//TODO Add layers to ignore
[ExecuteAlways]
public class RaycastFromBounds : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        [Serializable]
        public class RayBoundSettings
        {
            public int rayCount = 10;
            [Header("Shoot rays from bounds:")]
            public bool shootRaysFromTopBound = true;
            public bool shootRaysFromBottomBound = true;
            public bool shootRaysFromLeftBound = true;
            public bool shootRaysFromRightBound = true;
            [Header("Debugging")]
            [SerializeField, Disable]
            public Vector2 rayPadding;
            [SerializeField, Disable]
            public Vector2 sizeBounds,
                           sizeExtends,
                           topRightBounds,
                           bottomLeftBounds;

            public void CalculateSettingsFromCollider(Collider2D _col)
            {
                sizeBounds = _col.bounds.size;
                sizeExtends = _col.bounds.extents;
                rayCount = Mathf.Clamp(rayCount, 2, 99999);
                rayCount = Mathf.Clamp(rayCount, 2, 99999);
                rayPadding.x = sizeBounds.x / (rayCount - 1);
                rayPadding.y = sizeBounds.y / (rayCount - 1);
                topRightBounds =
                    _col.transform.position + new Vector3(sizeExtends.x, sizeExtends.y);
                bottomLeftBounds = _col.transform.position +
                                   new Vector3(-sizeExtends.x, -sizeExtends.y);
            }
        }

        [SerializeField]
        internal Collider2D col;
        [SerializeField]
        internal RayBoundSettings rayBoundSettings = new RayBoundSettings();
    }
    
    [Serializable]
    public class Events
    {
        [SerializeField]
        internal ConnectedRaycastHits2DQE connectedRaycastHits2DEvent = new ConnectedRaycastHits2DQE();
    }

    public delegate List<RichRaycastHit2D> ConnectedHitsProcessorDelegate(List<RichRaycastHit2D> richHits);

    public Settings settings = new Settings();
    public Events events = new Events();

    void Awake()
    {
        if (!settings.col)
            settings.col = this.GetComponent<Collider2D>();
    }

    void Update()
    {
        //Calculate ray settings
        settings.rayBoundSettings.CalculateSettingsFromCollider(settings.col);
        ShootRaysFromBounds(this.settings.rayBoundSettings, new List<Collider2D>(1) {settings.col},
                            ConnectedHitsProcessor);
    }

    /// <summary>
    /// Adds further information to the rich ray-cast hits and fires an event that releases the information to other components.
    /// </summary>
    /// <param name="richHits"></param>
    /// <returns></returns>
    private List<RichRaycastHit2D> ConnectedHitsProcessor(List<RichRaycastHit2D> richHits)
    {
        if (!Application.isPlaying) return null;
        if (!richHits.Any()) return null;
        richHits = ConnectedHitsEnricher(richHits);
        events.connectedRaycastHits2DEvent.Invoke(richHits.ToArray());
        return richHits;
    }

    /// <summary>
    /// Adds further information to the rich ray-cast hits.
    /// </summary>
    /// <param name="richHits"></param>
    /// <returns></returns>
    private List<RichRaycastHit2D> ConnectedHitsEnricher(List<RichRaycastHit2D> richHits)
    {
        richHits.ForEach(x =>
        {
            x.fromCol = settings.col;
            x.fromGO = settings.col.gameObject;
            x.fromRb = settings.col.attachedRigidbody;
        });
        return richHits;
    }

    /// <summary>
    /// Shoots rays from defined bounds.
    /// </summary>
    /// <param name="rayBoundSettings">Bounds defined by these settings.</param>
    /// <param name="unhittables">Colliders that should be ignored.</param>
    /// <param name="connectedHitsProcessor">A optional processor to enrich the info and/or throw events for rich ray-cast hits.</param>
    /// <returns></returns>
    public static List<RichRaycastHit2D> ShootRaysFromBounds(Settings.RayBoundSettings rayBoundSettings,
                                                              List<Collider2D> unhittables,
                                                              [CanBeNull] ConnectedHitsProcessorDelegate
                                                                  connectedHitsProcessor)
    {
        var totalHitsConnected = new List<RichRaycastHit2D>();

        //Shoot rays
        //Bottom
        if (rayBoundSettings.shootRaysFromBottomBound)
        {
            var o = rayBoundSettings.bottomLeftBounds;
            var rayCount = rayBoundSettings.rayCount;
            var rayPadding = rayBoundSettings.rayPadding.x;
            var dirPadding = new Vector2(1, 0);
            var rayDir = new Vector2(0, -1);
            var hits = ShootRaysInLine(rayCount, o, dirPadding, rayPadding, unhittables, rayDir, null);
            totalHitsConnected.AddRange(hits);
        }

        //Top
        if (rayBoundSettings.shootRaysFromTopBound)
        {
            var o = rayBoundSettings.topRightBounds;
            var rayCount = rayBoundSettings.rayCount;
            var rayPadding = rayBoundSettings.rayPadding.x;
            var dirPadding = new Vector2(-1, 0);
            var rayDir = new Vector2(0, 1);
            var hits = ShootRaysInLine(rayCount, o, dirPadding, rayPadding, unhittables, rayDir, null);
            totalHitsConnected.AddRange(hits);
        }

        //Left
        if (rayBoundSettings.shootRaysFromLeftBound)
        {
            var o = rayBoundSettings.bottomLeftBounds;
            var rayCount = rayBoundSettings.rayCount;
            var rayPadding = rayBoundSettings.rayPadding.y;
            var dirPadding = new Vector2(0, 1);
            var rayDir = new Vector2(-1, 0);
            var hits = ShootRaysInLine(rayCount, o, dirPadding, rayPadding, unhittables, rayDir, null);
            totalHitsConnected.AddRange(hits);
        }

        //Right
        if (rayBoundSettings.shootRaysFromRightBound)
        {
            var o = rayBoundSettings.topRightBounds;
            var rayCount = rayBoundSettings.rayCount;
            var rayPadding = rayBoundSettings.rayPadding.y;
            var dirPadding = new Vector2(0, -1);
            var rayDir = new Vector2(1, 0);
            var hits = ShootRaysInLine(rayCount, o, dirPadding, rayPadding, unhittables, rayDir, null);
            totalHitsConnected.AddRange(hits);
        }

        if (connectedHitsProcessor != null)
            totalHitsConnected = connectedHitsProcessor(totalHitsConnected);
        return totalHitsConnected;
    }

    /// <summary>
    /// Shoots rays in line with each other.
    /// </summary>
    /// <param name="rayCount">Amount of ray(s) to shoot.</param>
    /// <param name="o">Starting position of the line and first ray.</param>
    /// <param name="dirPadding">Direction of the ray padding.</param>
    /// <param name="rayPadding">Distance between each ray.</param>
    /// <param name="unhittables">Colliders that should be ignored.</param>
    /// <param name="rayDir">Direction of the ray(s).</param>
    /// <param name="connectedHitsProcessor">A optional processor to enrich the info and/or throw events for rich ray-cast hits.</param>
    /// <returns></returns>
    public static List<RichRaycastHit2D> ShootRaysInLine(int rayCount, Vector2 o, Vector2 dirPadding, float rayPadding,
                                                         List<Collider2D> unhittables,
                                                         Vector2 rayDir,
                                                         [CanBeNull]
                                                         ConnectedHitsProcessorDelegate connectedHitsProcessor)
    {
        var hits = new List<RichRaycastHit2D>();
        for (int i = 0; i < rayCount; i++)
        {
            var rayStart = o + i * dirPadding * rayPadding;
            var allHits = Physics2D.RaycastAll(rayStart, rayDir, Mathf.Infinity);
            if (allHits.Length < 0) continue;
            RaycastHit2D? hit = null;
            foreach (var aHit in allHits)
            {
                foreach (var unhittable in unhittables)
                {
                    if (aHit.collider.gameObject != unhittable.gameObject)
                    {
                        hit = aHit;
                        hits.Add(new RichRaycastHit2D(hit.Value, rayStart, rayDir));
                        break;
                    }
                }

                if (hit.HasValue)
                    break;
            }

            if (!hit.HasValue) continue;
            Debug.DrawLine(rayStart, hit.Value.point, Color.red);
        }

        if (connectedHitsProcessor != null)
            connectedHitsProcessor(hits);
        return hits;
    }
}