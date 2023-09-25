using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameConfigBean
{

    //角色X轴镜头移动速度
    public float speedForPlayerCameraMoveX = 2f;
    //角色Y轴镜头移动速度
    public float speedForPlayerCameraMoveY = 2f;

    //世界刷新范围
    public int worldRefreshRange = 5;
    //世界删除范围
    public int worldDestoryRange = 5;

    //实体显示具体
    public float entityShowDis = 20;
    //投射阴影范围
    public float shadowCastDis = 20;
}
