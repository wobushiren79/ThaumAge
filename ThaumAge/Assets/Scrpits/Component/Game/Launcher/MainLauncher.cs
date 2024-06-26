﻿using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class MainLauncher : BaseLauncher
{
    [Header("世界种子")]
    public int worldSeed = 19910824;
    [Header("世界范围")]
    public int worldRange = 1;

    public override void Launch()
    {
        base.Launch();
        IconHandler.Instance.InitData();

        UIHandler.Instance.OpenUIAndCloseOther<UILoading>();

        GameHandler.Instance.LoadGameResources(() =>
        {
            //设置游戏状态
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Main);
            //设置种子
            WorldCreateHandler.Instance.manager.SetWorldSeed(worldSeed);
            //设置远景模糊
            VolumeHandler.Instance.SetDepthOfField(GameStateEnum.Main);
            //设置世界类型为启动
            WorldCreateHandler.Instance.ChangeWorld(WorldTypeEnum.Launch, CompleteForUpdateChunk, Vector3Int.zero, worldRange);
        });
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        StartCoroutine(CoroutineForCompleteForUpdateChunk());
    }

    public IEnumerator CoroutineForCompleteForUpdateChunk()
    {
        while (!WorldCreateHandler.Instance.CheckAllInitChunkLoadComplete())
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.1f);
        //显示人物
        SceneMainHandler.Instance.ShowCharacter();
        //修改光照
        LightHandler.Instance.InitLight();
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
    }
}