using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingEditorManager : BaseManager,IBuildingInfoView
{
    public BuildingInfoController controllerForBuildingInfo;

    public Dictionary<Vector3Int, BuildingEditorModel> dicBlockBuild = new Dictionary<Vector3Int, BuildingEditorModel>();

    public GameObject objBlockContainer;
    public GameObject objPlane;
    public GameObject objBlockModel;

    //当前选中的方块信息
    public BlockInfoBean curSelectBlockInfo;
    //建造高度
    public int curBuildHigh = 0;
    //当前方块的方向
    public BlockDirectionEnum curBlockDirection = BlockDirectionEnum.UpForward;
    //当前建造模式 0创建 1删除
    public int curCreateTyp = 0;
    //当前的建筑信息
    public BuildingInfoBean curBuildingInfo;


    public void Awake()
    {
        objBlockContainer = GameObject.Find("BlockContainer");
        objPlane = GameObject.Find("Plane");
        objBlockModel = GameObject.Find("BlockModel");
        controllerForBuildingInfo = new BuildingInfoController(this, this);
    }

    #region 获取建筑数据回调
    public void GetBuildingInfoFail(string failMsg, Action action)
    {

    }

    public void GetBuildingInfoSuccess<T>(T data, Action<T> action)
    {

    }
    #endregion
}