using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeArmorStand : BlockBaseLink
{
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        listLinkPosition = new List<Vector3Int>() { Vector3Int.up };
    }

}