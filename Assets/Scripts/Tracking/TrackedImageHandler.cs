using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageHandler : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] XROrigin _xROrigin;
    [SerializeField] GameObject _placePrefab;
    
    [SerializeField] MarkerLoader _markerLoader;
    [SerializeField] private SearchPosition _searchPosition;
    
    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    
    private bool _isPlaneManagerEnabled = false;
    private void Start()
    {
        _arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        
    }
    
    private void OnDisable()
    {
        _arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> changedArgs)
    {
        foreach (ARTrackedImage image in changedArgs.added)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                if (!_isPlaneManagerEnabled)
                {
                    var planeManager = _arTrackedImageManager.GetComponent<ARPlaneManager>();
                    if (planeManager != null)
                    {
                        planeManager.enabled = true;
                        _isPlaneManagerEnabled = true;
                    }
                }

                var newMarker = Instantiate(_placePrefab, image.transform);
                _placeMarkers.Add(image.trackableId, newMarker);
                newMarker.transform.localPosition = new Vector3(0, 0, -0.01f);
                newMarker.transform.localRotation = Quaternion.identity;

                _markerLoader.LoadAndSpawnMarkers(image.transform);

                _searchPosition.SetTrackedImageTransform(image.transform);

            }
        }
    }
}
