using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GameHandler : BaseHandler<GameHandler, GameManager>
{
    public void LoadGameResources(Action callBack)
    {
        //禁用SRP 启用gpu实例化
        //GraphicsSettings.useScriptableRenderPipelineBatching = false;
        //加载世界资源
        WorldCreateHandler.Instance.manager.LoadResources(() => 
        {
            //加载方块资源
            BlockHandler.Instance.manager.LoadResources(() => 
            {
                callBack?.Invoke();
            });
        });
    }


    /// <summary>
    /// 初始化游戏角色
    /// </summary>
    public void InitCharacter(Vector3 characterPosition)
    {
        Player player = manager.player;
        //设置位置
        player.SetPosition(characterPosition);
        //刷新角色身上装备和皮肤
        player.RefreshCharacter();
    }
}