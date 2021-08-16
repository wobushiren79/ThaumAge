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
        //设置种子
        WorldCreateHandler.Instance.manager.SetWorldSeed(worldSeed);
        //刷新周围区块
        WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, worldRange, CompleteForUpdateChunk);
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //打开主UI
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.MainStart);
    }
}