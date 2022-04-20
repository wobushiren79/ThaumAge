
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
public class Test : BaseMonoBehaviour
{

    public void Start()
    {
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
    }

    private void OnGUI()
    {


    }



}



[BurstCompile]
public struct MyParallelJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }

}
