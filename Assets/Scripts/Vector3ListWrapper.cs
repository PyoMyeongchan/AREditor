using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3ListWrapper
{
    public List<MarkerData> markerDatas = new List<MarkerData>();
}

[System.Serializable]
public class MarkerData
{
    public string objectName;
    public Vector3 position;

    public MarkerData(string name, Vector3 pos)
    {
        objectName = name;
        position = pos;
    }
}


public class SaveMarkerData
{
    
    public void SaveVector3List(List<MarkerData> markerDatas)
    {
        Vector3ListWrapper wrapper = new Vector3ListWrapper();
        wrapper.markerDatas = markerDatas;

        string json = JsonUtility.ToJson(wrapper);

        string path = Application.persistentDataPath + "/marker3data.json";
        System.IO.File.WriteAllText(path, json);

        Debug.Log("저장 완료: " + path);
    }
}
