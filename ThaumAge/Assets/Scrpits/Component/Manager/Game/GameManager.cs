using UnityEditor;
using UnityEngine;

public class GameManager : BaseManager
{
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
}