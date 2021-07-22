using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureManager : BaseManager, ICharacterInfoView
{
    //角色头发列表
    public Dictionary<string, GameObject> dicCharacterHairModel = new Dictionary<string, GameObject>();
    public Dictionary<long, CharacterInfoBean> dicCharacterHairInfo = new Dictionary<long, CharacterInfoBean>();

    //角色眼睛列表
    public Dictionary<string, Texture2D> dicCharacterEyeTex = new Dictionary<string, Texture2D>();
    public Dictionary<long, CharacterInfoBean> dicCharacterEyeInfo = new Dictionary<long, CharacterInfoBean>();

    //角色嘴巴列表
    public Dictionary<string, Texture2D> dicCharacterMouthTex = new Dictionary<string, Texture2D>();
    public Dictionary<long, CharacterInfoBean> dicCharacterMouthInfo = new Dictionary<long, CharacterInfoBean>();

    //角色数据控制器
    protected CharacterInfoController controllerForCharacterInfo;

    private void Awake()
    {
        controllerForCharacterInfo = new CharacterInfoController(this, this);
        controllerForCharacterInfo.GetAllCharacterInfoHairData(InitCharacterInfoHair);
        controllerForCharacterInfo.GetAllCharacterInfoEyeData(InitCharacterInfoEye);
        controllerForCharacterInfo.GetAllCharacterInfoMouthData(InitCharacterInfoMouth);
    }

    /// <summary>
    /// 初始化角色发型信息
    /// </summary>
    /// <param name="listHairData"></param>
    protected void InitCharacterInfoHair(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterHairInfo, listData);
    }
    protected void InitCharacterInfoEye(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterEyeInfo, listData);
    }
    protected void InitCharacterInfoMouth(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterMouthInfo, listData);
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

    /// <summary>
    /// 获取眼睛贴图
    /// </summary>
    /// <param name="eyeName"></param>
    /// <returns></returns>
    public Texture2D GetCharacterEyeTex(string eyeName)
    {
        return GetModel(dicCharacterEyeTex, "character/eye", eyeName);
    }

    /// <summary>
    /// 获取嘴巴贴图
    /// </summary>
    /// <param name="mouthName"></param>
    /// <returns></returns>
    public Texture2D GetCharacterMouthTex(string mouthName)
    {
        return GetModel(dicCharacterEyeTex, "character/mouth", mouthName);
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