using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureManager : BaseManager, ICharacterInfoView
{
    //角色头发列表
    public Dictionary<string, GameObject> dicCharacterHairModel = new Dictionary<string, GameObject>();
    public Dictionary<long, CharacterInfoHairBean> dicCharacterHairInfo = new Dictionary<long, CharacterInfoHairBean>();

    //角色数据控制器
    protected CharacterInfoController controllerForCharacterInfo;

    private void Awake()
    {
        controllerForCharacterInfo = new CharacterInfoController(this, this);
        controllerForCharacterInfo.GetAllCharacterInfoData(InitCharacterInfoHair);
    }

    /// <summary>
    /// 初始化角色发型信息
    /// </summary>
    /// <param name="listHairData"></param>
    protected void InitCharacterInfoHair(List<CharacterInfoHairBean> listHairData)
    {
        dicCharacterHairInfo.Clear();
        for (int i = 0; i < listHairData.Count; i++)
        {
            CharacterInfoHairBean itemHairInfo = listHairData[i];
            dicCharacterHairInfo.Add(itemHairInfo.id, itemHairInfo);
        }
    }

    /// <summary>
    /// 获取角色头发模型
    /// </summary>
    /// <param name="hairName"></param>
    /// <returns></returns>
    public GameObject GetCharacterHairModel(string hairName)
    {
        return GetModel(dicCharacterHairModel, "character/hair", hairName);
    }

    #region 数据回调
    public void GetCharacterInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetCharacterInfoFail(string failMsg, Action action)
    {

    }
    #endregion
}