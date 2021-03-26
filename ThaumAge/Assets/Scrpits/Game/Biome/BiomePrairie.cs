using UnityEditor;
using UnityEngine;
public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight,  Vector3Int wPos)
    {
        return  BlockTypeEnum.Grass;
    }
}