using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeTombstone : BlockTypeChest
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0)
            chestSize = 2 * 7;
    }
}