using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingEditorManager : BaseManager
{
    public Dictionary<Vector3Int, BuildingEditorModel> dicBlockBuild = new Dictionary<Vector3Int, BuildingEditorModel>();

    public GameObject objBlockContainer;
    public GameObject objPlane;
    public GameObject objBlockModel;

    //当前选中的方块信息
    public BlockInfoBean curSelectBlockInfo;
    //建造高度
    public int curBuildHigh = 0;
    //当前方块的方向
    public BlockDirectionEnum curBlockDirection;
    //当前建造模式 0创建 1删除
    public int curCreateTyp = 0;

    public void Awake()
    {
        objBlockContainer = GameObject.Find("BlockContainer");
        objPlane = GameObject.Find("Plane");
        objBlockModel = GameObject.Find("BlockModel");
    }
}