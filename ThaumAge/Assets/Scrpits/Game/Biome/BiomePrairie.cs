using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTreeTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override void CreateBlockStructureForNormalTree(int blockId, Vector3Int baseWorldPosition)
    {
        BlockTypeEnum blockType = (BlockTypeEnum)blockId;
        if (blockType == BlockTypeEnum.TreeCherry)
        {
            BiomeCreateTreeTool.CreateNormalTree(baseWorldPosition, blockId, 6, 10, 2, (int)BlockTypeEnum.LeavesCherry);
        }
    }
}