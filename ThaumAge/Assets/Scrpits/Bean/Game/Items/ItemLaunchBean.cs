using System;
using UnityEditor;
using UnityEngine;

public class ItemLaunchBean
{
    //发射的道具ID
    public int itemId;

    //开始发射点
    public Vector3 launchStartPosition;

    //发射的向量
    public Vector3 launchDirection = Vector3.zero;

    //这个代表发射时的速度/力度等，可以通过此来模拟不同的力大小
    public int launchPower = 10;

    //发射状态 0：未发射  1发射中  2射中物体
    public int launchState = 0;

    //重力
    public Vector3 grity = new Vector3(0, -2, 0);

    //延迟删除时间
    public float timeForDestroy = 30;

    //检测的层级
    public int checkShotLayer = 1 << LayerInfo.ChunkCollider | 1 << LayerInfo.Creature | 1 << LayerInfo.Character;

    //检测半径范围
    public float checkShotRange = 0.1f;

    //射中目标的回调
    public Action<Collider> actionShotTarget;
}