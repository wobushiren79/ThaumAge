using UnityEditor;
using UnityEngine;

public class ItemMetaMagicCore : ItemBaseMeta
{
    //元素
    public int elemental;
    //创建方式
    public int create;
    //射程
    public int range;
    //范围
    public int scope;
    //威力
    public int power;

    public ElementalTypeEnum GetElement()
    {
        return (ElementalTypeEnum)elemental;
    }
}