using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaItemsTransition : BlockMetaBase
{
    //转换之前的物品
    public int itemBeforeId;
    public int itemBeforeNum;

    //转换之后的物品
    public int itemAfterId;
    public int itemAfterNum;

    //当前转换进度
    public float transitionPro;
}