﻿using UnityEditor;
using UnityEngine;

public class GameControlManager : BaseManager
{
    protected ControlForPlayer _controlForPlayer;

    protected ControlForCamera _controlForCamera;

    public ControlForPlayer controlForPlayer
    {
        get
        {
            if (_controlForPlayer == null)
            {
                _controlForPlayer = FindWithTag<ControlForPlayer>(TagInfo.Tag_Player);
            }
            return _controlForPlayer;
        }
    }

    public ControlForCamera controlForCamera
    {
        get
        {
            if (_controlForCamera == null)
            {
                _controlForCamera = FindWithTag<ControlForCamera>(TagInfo.Tag_GameControl);
            }
            return _controlForCamera;
        }
    }
}