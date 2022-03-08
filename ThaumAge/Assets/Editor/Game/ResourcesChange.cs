using UnityEditor;
using UnityEngine;

public class ResourcesChange : AssetPostprocessor
{
    public void OnPostprocessPrefab(GameObject changeObj)
    {
        Debug.Log("changeObj:"+ changeObj.name);
    }
}