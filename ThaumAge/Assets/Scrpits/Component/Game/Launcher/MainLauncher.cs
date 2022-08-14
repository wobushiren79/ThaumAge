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
        IconHandler.Instance.InitData(null);

        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);

        GameHandler.Instance.LoadGameResources(() =>
        {
            //设置游戏状态
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Main);
            //设置种子
            WorldCreateHandler.Instance.manager.SetWorldSeed(worldSeed);
            //设置世界类型为启动
            WorldCreateHandler.Instance.SetWorldType(WorldTypeEnum.Launch);
            //设置远景模糊
            VolumeHandler.Instance.SetDepthOfField(GameStateEnum.Main);

            
            //刷新周围区块
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, worldRange, true, CompleteForUpdateChunk);
        });
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //显示人物
        SceneMainHandler.Instance.ShowCharacter();
        //延迟3秒显示
        this.WaitExecuteSeconds(2, () =>
        {
            //打开主UI
            UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>(UIEnum.MainStart);
        });
    }
}