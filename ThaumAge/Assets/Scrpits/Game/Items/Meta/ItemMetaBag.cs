using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class ItemMetaBag : BlockMetaChest
{
    public ItemMetaBag() : base()
    {

    }

    public ItemMetaBag(int number) : base(number)
    {

    }

    public ItemMetaBag(int number, ItemsBean[] listData) : base(number, listData)
    {


    }
}