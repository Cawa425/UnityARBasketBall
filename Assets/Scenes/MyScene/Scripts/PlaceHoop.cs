using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceHoop : MonoBehaviour
{
    [SerializeField] [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    private GameObject hoopPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedHoop
    {
        get => hoopPrefab;
        set => hoopPrefab = value;
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    private GameObject spawnedHoop { get; set; }

    [SerializeField] [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    private GameObject ballPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedBall
    {
        get => ballPrefab;
        set => ballPrefab = value;
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedBall { get; private set; }

    private bool isHoopPlaced = false;


    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    private ARRaycastManager _mRaycastManager;

    private static readonly List<ARRaycastHit> SHits = new List<ARRaycastHit>();

    void Awake()
    {
        _mRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (isHoopPlaced) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (_mRaycastManager.Raycast(touch.position, SHits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = SHits[0].pose;

                    spawnedHoop = Instantiate(hoopPrefab, hitPose.position, Quaternion.AngleAxis(180, Vector3.up));
                    spawnedHoop.transform.parent = transform.parent;
                    Debug.Log("Колько заспавнено");
                    
                    
                    spawnedBall = Instantiate(ballPrefab, _mRaycastManager.transform.Find("AR Camera").gameObject.transform, true);
                    Debug.Log("Мяч заспавнен");
                    
                    isHoopPlaced = true;
                    
                    onPlacedObject?.Invoke();
                }
            }
        }
    }
}