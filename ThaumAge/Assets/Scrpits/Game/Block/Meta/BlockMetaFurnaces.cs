using UnityEditor;
using UnityEngine;

public class BlockMetaFurnaces : BlockMetaBase
{
    //当前剩余烧制时间
    public int fireTimeRemain;
    //当前烧制进度
    public float firePro;

    //火能源 来源
    public int itemFireSourceId;
    public int itemFireSourceNum;

    //烧制之前的物品
    public int itemBeforeId;
    public int itemBeforeNum;

    //烧制之后的物品
    public int itemAfterId;
    public int itemAfterNum;
}