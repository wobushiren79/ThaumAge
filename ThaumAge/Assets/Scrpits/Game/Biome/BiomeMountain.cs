using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeMountain : Biome
{
    protected int maxHight;
    protected int lineSnow;
    protected int lineHalfSnow;
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {

    }

    public override void CreateBlockStructureForNormalTreeSnow(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateNormalTreeSnow(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesCherry, 3, 6);
    }
}