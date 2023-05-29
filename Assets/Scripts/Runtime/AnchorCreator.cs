using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : PressInputBase
    {
        [SerializeField]
        GameObject m_Prefab;

        public GameObject pipe;

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

            // 1s后初始化消防栓
            Invoke(nameof(InitFireHydrant), 1f);

        }
        // 设置两个坐标，用于计算Unity坐标使用
        public LocationInfo origin;
        public LocationInfo target;
        public Text originGPSText;
        public Text targetGPSText;

        public void SetGPSInfo(String type)
        {
            switch (type)
            {
                case "origin":
                    origin = new LocationInfo
                    {
                        latitude = Input.location.lastData.latitude,
                        longitude = Input.location.lastData.longitude,
                        altitude = Input.location.lastData.altitude
                    };
                    originGPSText.text = $"latitude: {origin.latitude}\n" +
                        $"longitude: {origin.longitude}\n" +
                        $"altitude: {origin.altitude}\n";
                    break;
                case "target":
                    target = new LocationInfo
                    {
                        latitude = Input.location.lastData.latitude,
                        longitude = Input.location.lastData.longitude,
                        altitude = Input.location.lastData.altitude
                    };
                    targetGPSText.text = $"latitude: {target.latitude}\n" +
                        $"longitude: {target.longitude}\n" +
                        $"altitude: {target.altitude}\n";

                    // 院后门3个井盖的坐标
                    LocationInfo[] ptArr = new LocationInfo[]
                    {
                        new()
                        {
                            latitude = 30.29678046f,
                            longitude = 120.0894358f,
                            altitude = 12.905f

                        },
                        new()
                        {
                            latitude = 30.29677005f,
                            longitude = 120.0894966f,
                            altitude = 12.905f

                        },
                        new()
                        {
                           latitude = 30.29678294f,
                            longitude = 120.0895334f,
                            altitude = 12.9499f
                        }
                    };



                    UnityCoord result = gameObject.GetComponent<GPS2UnityCoord>().CalcUnityCoord(origin, target);
                    InitFireHydrant(new Vector3(result.X, result.Y, result.Z));
                    targetGPSText.text += $"target put!\n";


                    int index = 0;
                    foreach (LocationInfo pt in ptArr)
                    {
                        UnityCoord pt0Result = gameObject.GetComponent<GPS2UnityCoord>().CalcUnityCoord(origin, pt);
                        InitFireHydrant(new Vector3(pt0Result.X, pt0Result.Y, pt0Result.Z));
                        targetGPSText.text += $"p{index}: {pt0Result.X},{pt0Result.Y},{pt0Result.Z}put!\n";
                        index++;
                    }
                    break;
            }
        }

        public void PutPipe(Vector3 position)
        {
            var gameObject = Instantiate(pipe, position, m_Prefab.transform.rotation);
            ARAnchor anchor = ComponentUtils.GetOrAddIf<ARAnchor>(gameObject, true);
            m_Anchors.Add(anchor);
            targetGPSText.text += $"{Math.Round(position.x, 2)}, {Math.Round(position.y, 2)}, {Math.Round(position.z, 2)} has been put an fireHydrant！";
        }

        public void InitFireHydrant(Vector3 position)
        {
            var gameObject = Instantiate(m_Prefab, position, m_Prefab.transform.rotation);
            ARAnchor anchor = ComponentUtils.GetOrAddIf<ARAnchor>(gameObject, true);
            m_Anchors.Add(anchor);

            targetGPSText.text += $"{Math.Round(position.x, 2)}, {Math.Round(position.y, 2)}, {Math.Round(position.z, 2)} has been put an fireHydrant！";
        }
        public void InitFireHydrant()
        {
            // 以相机初始点作为坐标原点，y偏移1.3m，z偏移2.5m，生成一个消防栓
            var gameObject = Instantiate(m_Prefab, new Vector3(0, -1.3f, 2.5f), m_Prefab.transform.rotation);
            ARAnchor anchor = ComponentUtils.GetOrAddIf<ARAnchor>(gameObject, true);
            m_Anchors.Add(anchor);
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
        internal void permissionDeny(string permissionName)
        {
            Logger.Log($"{permissionName} denied");
        }
        internal void permissionDenyAskAgain(string permissionName)
        {
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
            }
            else
            {
                var curTimestamp = (long)Input.location.lastData.timestamp;
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(curTimestamp);
                DateTime dateTime = dateTimeOffset.LocalDateTime;
                if (totalTime >= 1)
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
            // 关闭点击生成锚点
            return;

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
