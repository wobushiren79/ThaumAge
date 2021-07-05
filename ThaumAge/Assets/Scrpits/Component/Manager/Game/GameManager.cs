using UnityEditor;
using UnityEngine;

public class GameManager : BaseManager
{
    protected GameStateEnum gameState = GameStateEnum.None;

    protected SOGameInitBean _gameInitData;

    protected Player _player;

    protected PlayerTargetBlock _playerTargetBlock;

    /// <summary>
    /// 游戏初始化数据
    /// </summary>
    public SOGameInitBean gameInitData
    {
        get
        {
            if (_gameInitData == null)
            {
                _gameInitData = LoadResourcesUtil.SyncLoadData<SOGameInitBean>("ScriptableObjects/GameInit");
            }
            return _gameInitData;
        }
    }


    /// <summary>
    /// 玩家
    /// </summary>
    public Player player
    {
        get
        {
            if (_player == null)
            {
                _player = FindWithTag<Player>(TagInfo.Tag_Player);
            }
            return _player;
        }
    }


    /// <summary>
    /// 玩家目标方块
    /// </summary>
    public PlayerTargetBlock playerTargetBlock
    {
        get
        {
            if (_playerTargetBlock == null)
            {
                _playerTargetBlock = FindWithTag<PlayerTargetBlock>(TagInfo.Tag_Player);
            }
            return _playerTargetBlock;
        }
    }

    /// <summary>
    /// 改变游戏状态
    /// </summary>
    /// <param name="gameState"></param>
    public void ChangeGameState(GameStateEnum gameState)
    {
        this.gameState = gameState;
    }


    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public GameStateEnum GetGameState()
    {
        return gameState;
    }
}