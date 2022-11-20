using UnityEditor;
using UnityEngine;

public class BlockMetaFocalManipulator : BlockMetaBase
{
    public ItemsBean itemMagicCore;

    //临时存储的数据 用于完成后赋值
    public ItemsBean itemMagicCoreWorkTemp;
    //改变进度
    public float workPro;
}