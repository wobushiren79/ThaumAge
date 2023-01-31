using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaInfernalFurnace : BlockMetaBaseLink
{
    //待烧制列表
    public List<long> listItem;
    //烧制进度
    public float transitionPro;
}