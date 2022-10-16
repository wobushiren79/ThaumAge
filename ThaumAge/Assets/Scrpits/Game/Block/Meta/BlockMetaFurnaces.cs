using UnityEditor;
using UnityEngine;

public class BlockMetaFurnaces : BlockMetaItemsTransition
{
    //当前剩余烧制时间
    public int fireTimeRemain;
    //最大存储烧制时间
    public int fireTimeMax;

    //火能源 来源
    public int itemFireSourceId;
    public int itemFireSourceNum;

    public BlockMetaFurnaces()
    {
        fireTimeMax = 200;
    }
}