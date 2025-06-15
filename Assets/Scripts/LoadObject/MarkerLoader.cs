using System.Collections.Generic;
using AREditor.LoadObject;
using UnityEngine;

public class MarkerLoader : MonoBehaviour
{ 
    [SerializeField] private GameObject markerPrefab;
    private SaveMarkerData _saveMarkerData;
    [SerializeField] private SavePosition savePosition;

    private void Awake()
    {
        _saveMarkerData = new SaveMarkerData();
    }
    
    private List<GameObject> spawnedMarkers = new List<GameObject>();

    public void LoadAndSpawnMarkers()
    {
        foreach (var obj in spawnedMarkers)
        {
            Destroy(obj);
        }
        spawnedMarkers.Clear();
    
        List<MarkerData> markerDatas = _saveMarkerData.LoadMarkerList();

        foreach (var data in markerDatas)
        {
            GameObject marker = Instantiate(markerPrefab, data.position, Quaternion.identity);
            marker.name = data.objectName;

            var idHolder = marker.GetComponent<MarkerIDHolder>();
            if (idHolder != null)
            {
                idHolder.markerId = data.id;

            }
            savePosition.markerDatas.Add(data);
            
            spawnedMarkers.Add(marker);
        }
    }
} 
