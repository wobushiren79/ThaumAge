using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomeForestMagic : Biome
{

    //魔法深林
    public BiomeForestMagic() : base(BiomeTypeEnum.ForestMagic)
    {
        
    }

    protected void AddMushroomTree(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        //AddBuilding(0.0001f, 101, startPosition, BuildingTypeEnum.MushrooBig);
        //AddBuilding(0.0001f, 201, startPosition, BuildingTypeEnum.Mushroom);
        //AddBuilding(0.0001f, 301, startPosition, BuildingTypeEnum.MushrooSmall);
    }


    protected void AddBigTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeSilver,
            treeLeaves = BlockTypeEnum.LeavesSilver,
            leavesRange = 4,
        };
        BiomeCreateTreeTool.AddTreeForBig(501, wPos, treeData);
    }

    protected bool AddWorldTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.0001f,
            minHeight = 30,
            maxHeight = 50,
            treeTrunk = BlockTypeEnum.TreeWorld,
            treeLeaves = BlockTypeEnum.LeavesWorld,
            leavesRange = 4,
            trunkRange = 3,
        };
        return BiomeCreateTreeTool.AddTreeForWorld(502, wPos, treeData);
    }
}