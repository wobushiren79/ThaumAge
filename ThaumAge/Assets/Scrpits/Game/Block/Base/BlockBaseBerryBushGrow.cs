using UnityEditor;
using UnityEngine;

public class BlockBaseBerryBushGrow : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        int growId = blockInfo.remark_int;
        chunk.SetBlockForLocal(localPosition, (BlockTypeEnum)growId);
    }
}