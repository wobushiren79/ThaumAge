using UnityEditor;
using UnityEngine;

public class GameDataManager : BaseManager,IGameConfigView
{    
    //游戏设置
    public GameConfigBean gameConfig;

    public  GameConfigController controllerForGameConfig;

    protected void Awake()
    {
        controllerForGameConfig = new GameConfigController(this, this);
        controllerForGameConfig.GetGameConfigData();
    }

    /// <summary>
    /// 保存游戏设置
    /// </summary>
    public void SaveGameConfig()
    {
        controllerForGameConfig.SaveGameConfigData(gameConfig);
    }

    /// <summary>
    /// 获取游戏设置
    /// </summary>
    /// <returns></returns>
    public GameConfigBean GetGameConfig()
    {
        if (gameConfig == null)
            gameConfig = new GameConfigBean();
        return gameConfig;
    }

    #region 游戏设置数据回掉
    public void GetGameConfigFail()
    {

    }

    public void GetGameConfigSuccess(GameConfigBean configBean)
    {
        gameConfig = configBean;
    }

    public void SetGameConfigFail()
    {

    }

    public void SetGameConfigSuccess(GameConfigBean configBean)
    {

    }
    #endregion
}