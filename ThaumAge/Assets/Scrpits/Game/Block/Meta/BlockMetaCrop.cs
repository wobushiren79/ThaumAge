using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaCrop : BlockMetaBase
{
    //生长进度初始0 每次+1
    public int growPro;
    //是否开始生长 用于第一次放置时 隔1分钟再检测生长
    public bool isStartGrow;
    //等级 用于多级植物 0为最底下的基础植物 
    public int level;
}