using UnityEditor;
using UnityEngine;

public class BlockTypeInfernalFurnace : BlockBaseLinkLarge
{
    public override BuildingTypeEnum GetBuildingType()
    {
        return BuildingTypeEnum.InfernalFurnace;
    }
}