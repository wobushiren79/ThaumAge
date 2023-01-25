using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BlockMetaBaseLink : BlockMetaBase
{
    //等级 0为主方块 1为附属方块
    public int level;
    //主方块位置
    public Vector3IntBean linkBasePosition;
    //主方块类型
    public int baseBlockType;
    //是否摧毁时摧毁所有方块(默认true)
    public bool isBreakAll = true;
    //是否有破坏的mesh（默认有）
    public bool isBreakMesh = true;
    public Vector3Int GetBasePosition()
    {
        return linkBasePosition.GetVector3Int();
    }
}