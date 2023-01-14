using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaFocalManipulator : BlockMetaBase
{
    public ItemsBean itemMagicCore = new ItemsBean();

    //临时存储的数据 用于完成后赋值
    public ItemsBean itemMagicCoreWorkTemp = new ItemsBean();
    //改变进度
    public float workPro;
}