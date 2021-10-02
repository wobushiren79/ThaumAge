﻿using UnityEngine;
using UnityEditor;

public enum UIEnum
{
    MainStart = 1001, //主界面-开始
    MainCreate = 1002,//主界面-创建游戏
    MainUserData = 1003,//主界面-继续游戏
    MainMaker = 1008,//主界面-制作人界面
    MainExit = 1009,//主界面-离开

    GameMain = 2001,//游戏-主界面
    GameUserDetails = 2002,//游戏-用户详情
    GameSetting = 2999,//游戏-设置

    GodMain = 9001,//GM-主界面
}