using UnityEditor;
using UnityEngine;
using System;
    
[Serializable]
public class MagicBean
{
    //创建方式
    public int magicType;
    //元素
    public int element;
    //魔法移动速度
    public float magicSpeed;
    //创建位置
    public Vector3 createPosition;
    //发射方向
    public Vector3 direction;
    //生命周期（时间到达后毁灭）
    public int lifeTime = 60;

    /// <summary>
    /// 获取魔法类型
    /// </summary>
    public MagicTypeEnum GetMagicType()
    {
        return (MagicTypeEnum)magicType;
    }

    /// <summary>
    /// 获取元素类型
    /// </summary>
    /// <returns></returns>
    public ElementalTypeEnum GetElementalType()
    {
        return (ElementalTypeEnum)element;
    }
}