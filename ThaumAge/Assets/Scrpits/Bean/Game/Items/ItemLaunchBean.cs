using UnityEditor;
using UnityEngine;

public class ItemLaunchBean
{
    //发射的道具ID
    public int itemId;

    //发射的向量
    public Vector3 launchDirection = Vector3.zero;

    //这个代表发射时的速度/力度等，可以通过此来模拟不同的力大小
    public int launchPower = 10;

    //发射状态 0：未发射  1发射中  2射中物体
    public int launchState = 0;

    //重力
    public Vector3 grity = new Vector3(0, -10, 0);
}