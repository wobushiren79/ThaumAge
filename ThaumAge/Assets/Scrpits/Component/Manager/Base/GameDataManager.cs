using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GameDataManager : BaseManager,
    IGameConfigView, IWorldDataView, IUserDataView
{
    //游戏设置
    public GameConfigBean gameConfig;
    public UserDataBean userData;

    public GameConfigController controllerForGameConfig;
    public WorldDataController controllerForWorldData;
    public UserDataController controllerForUserData;

    protected void Awake()
    {
        controllerForGameConfig = new GameConfigController(this, this);
        controllerForWorldData = new WorldDataController(this, this);
        controllerForUserData = new UserDataController(this, this);
        controllerForGameConfig.GetGameConfigData();
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public UserDataBean GetUserData()
    {
        if (userData == null)
            userData = new UserDataBean();
        return userData;
    }

    /// <summary>
    /// 获取世界数据
    /// </summary>
    /// <returns></returns>
    public WorldDataBean GetWorldData(string userId, WorldTypeEnum worldType,Vector3Int position)
    {
        return controllerForWorldData.GetWorldData( userId,  worldType, position,  null);
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

    /// <summary>
    /// 保存游戏设置
    /// </summary>
    public void SaveGameConfig()
    {
        controllerForGameConfig.SaveGameConfigData(gameConfig);
    }

    /// <summary>
    /// 异步保存游戏数据
    /// </summary>
    public async void SaveGameDataAsync(WorldDataBean worldData)
    {
        await Task.Run(() =>
        {
            lock (this)
            {
                worldData.chunkData.SaveData();
                controllerForUserData.SetUserData(userData);
                controllerForWorldData.SetWorldData(worldData, null);
            }
        });
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void DeletGameData(string userId)
    {
        controllerForUserData.RemoveUserData(userId);
    }
    public void DeletGameData()
    {
        DeletGameData(userData.userId);
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

    public void GetWorldDataSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetWorldDataFail(string failMsg, Action action)
    {
    }

    public void GetUserDataSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetUserDataFail(string failMsg, Action action)
    {
    }
    #endregion
}