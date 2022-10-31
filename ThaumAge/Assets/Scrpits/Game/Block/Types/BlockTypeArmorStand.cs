using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeArmorStand : BlockBaseLink
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        listLinkPosition = new List<Vector3Int>() { Vector3Int.up };
    }
}