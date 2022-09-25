using UnityEditor;
using UnityEngine;

public class BlockMetaSapling : BlockMetaBase
{
    //生长进度初始0 每次+1
    public int growPro;
    //是否开始生长 用于第一次放置时 隔1分钟再检测生长
    public bool isStartGrow;
}