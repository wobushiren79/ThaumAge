using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BlockDoorBean : BlockBaseLinkBean
{
    //状态 0关门 1开门
    public int state;
}