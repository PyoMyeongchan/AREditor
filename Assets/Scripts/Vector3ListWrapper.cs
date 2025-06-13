using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3ListWrapper
{
    public List<Vector3> Positions = new List<Vector3>();
}

public class SaveMarkerData
{
    
    public void SaveVector3List(List<Vector3> vectors)
    {
        Vector3ListWrapper wrapper = new Vector3ListWrapper();
        wrapper.Positions = vectors;

        string json = JsonUtility.ToJson(wrapper);

        string path = Application.persistentDataPath + "/vector3data.json";
        System.IO.File.WriteAllText(path, json);

        Debug.Log("저장 완료: " + path);
    }
}
