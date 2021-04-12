using UnityEditor;
using UnityEngine;

public class GameManager : BaseManager
{
    public GameStateEnum gameState = GameStateEnum.None;

    protected Player _player;

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
    /// 改变游戏状态
    /// </summary>
    /// <param name="gameState"></param>
    public void ChangeGameState(GameStateEnum gameState)
    {
        this.gameState = gameState;
    }

    public GameStateEnum GetGameState()
    {
        return gameState;
    }
}