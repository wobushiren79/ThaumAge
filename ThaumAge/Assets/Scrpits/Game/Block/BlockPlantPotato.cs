using UnityEditor;
using UnityEngine;

public class BlockPlantPotato :BlockShapePlantCross
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.InitBlock(chunk, localPosition);
        chunk.RegisterEventUpdate(localPosition, 60);
    }

    public override void EventBlockUpdateFor60(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateFor60(chunk, localPosition);

    }
}