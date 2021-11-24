using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GameDataManager : BaseManager,
    IGameConfigView, IChunkSaveView, IUserDataView,IBaseDataView
{
    //游戏设置
    public GameConfigBean gameConfig;
    public UserDataBean userData;

    public GameConfigController controllerForGameConfig;
    public ChunkSaveController controllerForChunkSave;
    public UserDataController controllerForUserData;
    public BaseDataController controllerForBase;

    protected static object lockForSaveData = new object();

    protected void Awake()
    {
        controllerForGameConfig = new GameConfigController(this, this);
        controllerForChunkSave = new ChunkSaveController(this, this);
        controllerForUserData = new UserDataController(this, this);
        controllerForBase = new BaseDataController(this, this);
        controllerForGameConfig.GetGameConfigData();
    }

    /// <summary>
    /// 获取所有的用户数据
    /// </summary>
    /// <returns></returns>
    public void GetAllUserData(Action<List<UserDataBean>> action)
    {
        controllerForUserData.GetAllUserDataData(action);
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public UserDataBean GetUserData()
    {
        if (userData == null)
        {
            userData = new UserDataBean();
            userData.timeForGame.hour = 6;
        }  
        return userData;
    }

    /// <summary>
    /// 使用该数据
    /// </summary>
    public void UseUserData(UserDataBean userData)
    {
        this.userData = userData;
    }

    /// <summary>
    /// 获取世界数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveBean GetChunkSaveData(string userId, WorldTypeEnum worldType,Vector3Int position)
    {
        return controllerForChunkSave.GetChunkSaveData( userId,  worldType, position,  null);
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
    public async void SaveGameDataAsync(ChunkSaveBean chunkSaveData)
    {
        await Task.Run(() =>
        {
            lock (lockForSaveData)
            {
                chunkSaveData.SaveData();
                controllerForUserData.SetUserData(userData);
                controllerForChunkSave.SetChunkSaveData(chunkSaveData, null);
            }
        });
    }

    /// <summary>
    /// 保存用户数据
    /// </summary>
    /// <param name="userData"></param>
    public void SaveUserData(UserDataBean userData)
    {
        controllerForUserData.SetUserData(userData);
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

    public void GetUserDataSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetUserDataFail(string failMsg, Action action)
    {

    }

    public void GetAllBaseDataSuccess(List<BaseDataBean> listData)
    {

    }

    public void GetAllBaseDataFail(string failMsg)
    {

    }

    public void GetChunkSaveSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetChunkSaveFail(string failMsg, Action action)
    {

    }
    #endregion
}