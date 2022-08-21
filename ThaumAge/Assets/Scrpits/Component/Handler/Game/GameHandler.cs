using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameHandler : BaseHandler<GameHandler, GameManager>
{
    //当前的启动器
    public BaseLauncher launcher;

    //场景启动
    public Action actionForLauncher;

    /// <summary>
    /// 加载游戏资源
    /// </summary>
    /// <param name="callBack"></param>
    public void LoadGameResources(Action callBack)
    {
        //禁用SRP 启用gpu实例化
        //GraphicsSettings.useScriptableRenderPipelineBatching = false;
        //加载世界资源
        WorldCreateHandler.Instance.manager.LoadResources(() =>
        {
            Action completeForLoadBiomeResources = () =>
            {
                callBack?.Invoke();
            };
            Action completeForLoadBlockResources = () =>
            {
                //加载生态资源
                BiomeHandler.Instance.manager.LoadResources(completeForLoadBiomeResources);
            };

            //加载方块资源
            BlockHandler.Instance.manager.LoadResources(completeForLoadBlockResources);
        });
    }


    /// <summary>
    /// 初始化游戏角色
    /// </summary>
    public void InitCharacter()
    {
        Player player = manager.player;
        //设置位置
        player.InitPosition();
        //刷新角色身上装备和皮肤
        player.RefreshCharacter();
    }
}