using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureManager : BaseManager, ICharacterInfoView
{
    public readonly string pathHair = "Assets/Texture/Character/Hair";
    public readonly string pathEye = "Assets/Texture/Character/Eye";
    public readonly string pathMouth = "Assets/Texture/Character/Mouth";
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

    /// <summary>
    /// 初始化角色眼睛
    /// </summary>
    /// <param name="listData"></param>
    protected void InitCharacterInfoEye(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterEyeInfo, listData);
    }

    /// <summary>
    /// 初始角色嘴巴
    /// </summary>
    /// <param name="listData"></param>
    protected void InitCharacterInfoMouth(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterMouthInfo, listData);
    }

    /// <summary>
    /// 获取角色发型信息
    /// </summary>
    /// <param name="id"></param>
    public CharacterInfoBean GetCharacterInfoHair(long id)
    {
        return GetDataById(id, dicCharacterHairInfo);
    }

    /// <summary>
    /// 获取角色眼睛信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterInfoBean GetCharacterInfoEye(long id)
    {
        return GetDataById(id, dicCharacterEyeInfo);
    }

    /// <summary>
    /// 获取角色眼睛信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<CharacterInfoBean> GetCharacterInfoEye(List<long> ids)
    {
        List<CharacterInfoBean> listData = new List<CharacterInfoBean>();
        for (int i = 0; i < ids.Count; i++)
        {
            CharacterInfoBean itemData = GetCharacterInfoEye(ids[i]);
            listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 获取角色嘴巴信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterInfoBean GetCharacterInfoMouth(long id)
    {
        return GetDataById(id, dicCharacterMouthInfo);
    }

    /// <summary>
    /// 获取角色嘴巴信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<CharacterInfoBean> GetCharacterInfoMouth(List<long> ids)
    {
        List<CharacterInfoBean> listData = new List<CharacterInfoBean>();
        for (int i = 0; i < ids.Count; i++)
        {
            CharacterInfoBean itemData = GetCharacterInfoMouth(ids[i]);
            listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 获取角色头发模型
    /// </summary>
    /// <param name="hairName"></param>
    /// <returns></returns>
    public void GetCharacterHairModel(string hairName, Action<GameObject> callBack)
    {
        GetModelForAddressables(dicCharacterHairModel, $"{pathHair}/{hairName}", callBack);
    }

    /// <summary>
    /// 获取眼睛贴图
    /// </summary>
    /// <param name="eyeName"></param>
    /// <returns></returns>
    public void GetCharacterEyeTex(string eyeName, Action<Texture2D> callBack)
    {
        GetModelForAddressables(dicCharacterEyeTex, $"{pathEye}/{eyeName}.png", callBack);
    }

    /// <summary>
    /// 获取嘴巴贴图
    /// </summary>
    /// <param name="mouthName"></param>
    /// <returns></returns>
    public void GetCharacterMouthTex(string mouthName, Action<Texture2D> callBack)
    {
        GetModelForAddressables(dicCharacterMouthTex, $"{pathMouth}/{mouthName}.png", callBack);
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