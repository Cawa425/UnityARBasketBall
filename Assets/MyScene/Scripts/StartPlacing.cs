using System;
using System.Collections.Generic;
using Scenes.UX.MyScene;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MyScene.Scripts
{
    [RequireComponent(typeof(ARRaycastManager))]
    public class StartPlacing : MonoBehaviour
    {
        [SerializeField] [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        private GameObject hoopPrefab;

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        private GameObject spawnedHoop { get; set; }

        [SerializeField] [Tooltip("Instantiates after touch.")]
        private GameObject ballPrefab;
        

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        private GameObject SpawnedBall { get; set; }

        private bool isHoopPlaced = false;


        /// <summary>
        /// Invoked whenever an object is placed in on a plane.
        /// </summary>
        public static event Action onPlacedObject;

        private ARRaycastManager _mRaycastManager;

        private static readonly List<ARRaycastHit> SHits = new List<ARRaycastHit>();//точки в мире 

        void Awake()
        {
            _mRaycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {
            if (isHoopPlaced) return;

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (_mRaycastManager.Raycast(touch.position, SHits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = SHits[0].pose;

                        spawnedHoop = Instantiate(hoopPrefab, hitPose.position, Quaternion.AngleAxis(180, Vector3.up));
                        Debug.Log("Кольцо заспавнено");
                    
                    
                        SpawnedBall = Instantiate(ballPrefab, _mRaycastManager.transform.Find("AR Camera").gameObject.transform, true);
                        Debug.Log("Мяч заспавнен");
                    
                        isHoopPlaced = true;
                        
                        // включаем худ
                        myUIManager.SettingsList.ForEach(x=>x.SetActive(true));

                        onPlacedObject?.Invoke();
                    }
                }
            }
        }
    }
}