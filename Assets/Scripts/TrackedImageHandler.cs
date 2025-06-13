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
    [SerializeField] private XROrigin _xROrigin;
    private Dictionary<TrackableId, GameObject> _placeMarkers = new Dictionary<TrackableId, GameObject>();
    [SerializeField] GameObject _placePrefab;
    private GameObject _placePrefabInstance;
    

    private void Start()
    {
        _arTrackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> changedArgs)
    {
        foreach (ARTrackedImage image in changedArgs.added)
        {
            GameObject object1 = null; //추가
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _arTrackedImageManager.GetComponent<ARPlaneManager>().enabled = true;
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }

            _placeMarkers[image.trackableId].transform.position = image.transform.position;
            _placeMarkers[image.trackableId].transform.rotation = image.transform.rotation;
            _placePrefabInstance = _placeMarkers[image.trackableId];
            
        }

        foreach (ARTrackedImage image in changedArgs.updated)
        {
            if (_placeMarkers.TryGetValue(image.trackableId, out var placeMarker) == false)
            {
                _placeMarkers.Add(image.trackableId, Instantiate(_placePrefab));
            }

            _placeMarkers[image.trackableId].transform.position = image.transform.position;
            _placeMarkers[image.trackableId].transform.rotation = image.transform.rotation;
            _placePrefabInstance = _placeMarkers[image.trackableId];
        }
    }

}
