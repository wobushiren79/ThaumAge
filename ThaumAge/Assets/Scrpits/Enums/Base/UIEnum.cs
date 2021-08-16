using UnityEngine;
using UnityEditor;

public enum UIEnum
{
    MainStart = 1001, //主界面-开始
    MainCreate = 1002,//主界面-新游戏
    MainContinue = 1003,//主界面-继续
    MainExit = 1009,//主界面-离开

    GameMain = 2001,//游戏-主界面
    GameUserDetails = 2002,//游戏-用户详情
    GameSettings = 2999,//游戏-设置

    GodItems = 9001,//GM-所有道具
}