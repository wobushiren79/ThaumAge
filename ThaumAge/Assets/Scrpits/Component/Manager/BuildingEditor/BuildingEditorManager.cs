using UnityEditor;
using UnityEngine;

public class BuildingEditorManager : BaseManager
{
    public GameObject objBlockContainer;
    public GameObject objPlane;
    public GameObject objBlockModel;

    //当前选中的方块信息
    public BlockInfoBean curSelectBlockInfo;
    //建造高度
    public int curBuildHigh = 0;
    //当前方块的方向
    public BlockDirectionEnum curBlockDirection;

    public void Awake()
    {
        objBlockContainer = GameObject.Find("BlockContainer");
        objPlane = GameObject.Find("Plane");
        objBlockModel = GameObject.Find("BlockModel");
    }
}