using UnityEditor;
using UnityEngine;

public enum GameBookMapItemStateEnum
{
    Lock = 0,//未解锁
    UnlockUndone = 1,//已解锁未完成
    UnlockDone = 2,//已解锁已完成
}