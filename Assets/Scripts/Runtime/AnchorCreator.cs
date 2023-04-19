using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : PressInputBase
    {
        [SerializeField]
        GameObject m_Prefab;

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public void RemoveAllAnchors()
        {
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
        }

        protected override void Awake()
        {
            base.Awake();

            Input.location.Start(1f, 0.1f);
            if (!CheckLocationPermission())
            {
                Logger.Log("Location Permission Missing!");
            }

            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();

            if (m_AnchorManager.subsystem == null)
            {
                enabled = false;
                Debug.LogWarning($"No active XRAnchorSubsystem is available, so {typeof(AnchorCreator).FullName} will not be enabled.");
            }
        }

        void SetAnchorText(ARAnchor anchor, string text)
        {
            var canvasTextManager = anchor.GetComponent<CanvasTextManager>();
            if (canvasTextManager)
            {
                canvasTextManager.text = text;
            }
        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager != null)
                {
                    Logger.Log("Creating anchor attachment.");

                    if (m_Prefab != null)
                    {
                        var oldPrefab = m_AnchorManager.anchorPrefab;
                        m_AnchorManager.anchorPrefab = m_Prefab;
                        anchor = m_AnchorManager.AttachAnchor(plane, new Pose(hit.pose.position, m_Prefab.transform.rotation));
                        m_AnchorManager.anchorPrefab = oldPrefab;
                    }
                    else
                    {
                        anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    }

                    SetAnchorText(anchor, $"Attached to plane {plane.trackableId}");
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            Logger.Log("Creating regular anchor.");

            if (m_Prefab != null)
            {
                // Note: the anchor can be anywhere in the scene hierarchy
                var gameObject = Instantiate(m_Prefab, hit.pose.position, m_Prefab.transform.rotation);

                // Make sure the new GameObject has an ARAnchor component
                anchor = ComponentUtils.GetOrAddIf<ARAnchor>(gameObject, true);
            }
            else
            {
                var gameObject = new GameObject("Anchor");
                gameObject.transform.SetPositionAndRotation(hit.pose.position, hit.pose.rotation);
                anchor = gameObject.AddComponent<ARAnchor>();
            }

            SetAnchorText(anchor, $"Anchor (from {hit.hitType})");

            return anchor;
        }

        internal void permissionGranted(string permissionName)
        {
            Logger.Log($"{permissionName} granted!");
            Input.location.Start(1f, 0.1f);
        }
        internal void permissionDeny(string permissionName){
            Logger.Log($"{permissionName} denied");
        }
        internal void permissionDenyAskAgain(string permissionName){
            Logger.Log($"{permissionName} denyAskAgain");
        }
        private bool CheckLocationPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionGranted += permissionGranted;
                callbacks.PermissionDenied += permissionDeny;
                callbacks.PermissionDeniedAndDontAskAgain += permissionDenyAskAgain;
                Permission.RequestUserPermission(Permission.FineLocation, callbacks);
                return false;
            }

            return true;
        }
        
        private long lastTimestamp = 0;
        private float totalTime = 0;
        private String activeStatus = "offline";
        private void Update()
        {
            totalTime += Time.deltaTime;
            if (!CheckLocationPermission())
            {
                Logger.Log("Location Permission Missing!");
            } else {
                var curTimestamp= (long)Input.location.lastData.timestamp;
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(curTimestamp);
                DateTime dateTime = dateTimeOffset.LocalDateTime;
                if(totalTime >= 1)
                {
                    bool isActive = lastTimestamp != curTimestamp;
                    locationInfoText.color = isActive ? new Color(255, 255, 255) : new Color(255, 0, 0);
                    activeStatus = isActive ? "active" : "offline";
                    lastTimestamp = curTimestamp;
                    totalTime = 0;
                }

                locationInfoText.text = $"Location status:  {Input.location.status} ({activeStatus})\n" +
                    $"latitude: {Input.location.lastData.latitude} \n" +
                    $"longitude: {Input.location.lastData.longitude} \n" +
                    $"altitude: {Input.location.lastData.altitude} \n" +
                    $"horizontalAccuracy: {Input.location.lastData.horizontalAccuracy} \n" +
                    $"verticalAccuracy: {Input.location.lastData.verticalAccuracy} \n" +
                    $"timestamp: {dateTime:yyyy-MM-dd HH:mm:ss}";
            }
        }
        public Text locationInfoText;

        protected override void OnPress(Vector3 position)
        {
            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (m_RaycastManager.Raycast(position, s_Hits, trackableTypes))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor != null)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
