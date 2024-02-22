using UnityEditor;
using UnityEngine;

public class BuildingBaseType
{
    public BuildingInfoBean buildingInfo;

    /// <summary>
    /// 创建建筑
    /// </summary>
    public virtual void CreateBuilding(int blockId, Vector3Int baseWorldPosition)
    {
        if (buildingInfo != null)
        {
            BiomeCreateTool.AddBuilding(baseWorldPosition, buildingInfo);
        }
    }
}