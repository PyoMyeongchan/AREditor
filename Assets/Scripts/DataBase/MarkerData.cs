using UnityEngine;
using System;

[System.Serializable]
public class MarkerData
{
    public string id;
    public string objectName;
    public Vector3 position;
    
    public MarkerData(string objectName, Vector3 position)
    {
        this.id = Guid.NewGuid().ToString(); // 생성 시 자동 부여
        this.objectName = objectName;
        this.position = position;
    }
    
    public MarkerData(string id,string name, Vector3 pos)
    {
        this.id = id;
        objectName = name;
        position = pos;
    }
}
