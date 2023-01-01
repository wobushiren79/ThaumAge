using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeTombstone : BlockTypeBox
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0)
            boxSize = 2 * 7;
    }

    public override void OpenBox(Vector3Int worldPosition)
    {

    }

    public override void CloseBox(Vector3Int worldPosition)
    {

    }
}