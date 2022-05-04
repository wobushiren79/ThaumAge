using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BlockMetaDoor : BlockMetaBaseLink
{
    //状态 0关门 1开门
    public int state;
}