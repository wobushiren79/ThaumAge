using UnityEditor;
using UnityEngine;

public class ItemMetaGolemCore : ItemBaseMeta
{
    //绑定的世界方块坐标
    public Vector3Int bindBlockWorldPosition = new Vector3Int(0, int.MaxValue, 0);
}