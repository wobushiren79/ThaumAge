using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class MagicManager : BaseManager
{
    //魔法特效预制路径
    public static string PathMagicCpt = "Assets/Prefabs/Game/Magic";
    //魔法预制列表
    public Dictionary<string, GameObject> dicMagicCpt = new Dictionary<string, GameObject>();

    /// <summary>
    /// 获取魔法预制
    /// </summary>
    /// <param name="magicData"></param>
    /// <param name="callBack"></param>
    public void GetMagicCpt(MagicBean magicData,string magicCptName, Action<GameObject> callBack)
    {
        ElementalTypeEnum elementalType = magicData.GetElementalType();
        string keyName = $"{PathMagicCpt}/{magicCptName}";
        GetModelForAddressables(dicMagicCpt, keyName, (objModel) =>
        {
            GameObject objMagic = Instantiate(gameObject, objModel);
            objMagic.ShowObj(true);
            objMagic.transform.position = magicData.createPosition;
            objMagic.AddComponentEX<MagicCpt>();
            callBack?.Invoke(objMagic);
        });
    }
}