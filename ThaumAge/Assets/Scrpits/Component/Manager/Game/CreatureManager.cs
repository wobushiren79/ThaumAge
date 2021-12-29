using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureManager : BaseManager,
    ICharacterInfoView,ICreatureInfoView
{
    public readonly string pathHair = "Assets/Prefabs/Model/Character/Hair";
    public readonly string pathEye = "Assets/Texture/Character/Eye";
    public readonly string pathMouth = "Assets/Texture/Character/Mouth";
    public readonly string pathSkin = "Assets/Texture/Character/Skin";

    public readonly string pathCreature = "Assets/Prefabs/Model/Creature";
    public readonly string pathCreatureLifeProgress = "Assets/Prefabs/Model/Creature";
    //角色头发列表
    public Dictionary<string, GameObject> dicCharacterHairModel = new Dictionary<string, GameObject>();
    public Dictionary<long, CharacterInfoBean> dicCharacterHairInfo = new Dictionary<long, CharacterInfoBean>();

    //角色眼睛列表
    public Dictionary<string, Texture2D> dicCharacterEyeTex = new Dictionary<string, Texture2D>();
    public Dictionary<long, CharacterInfoBean> dicCharacterEyeInfo = new Dictionary<long, CharacterInfoBean>();

    //角色嘴巴列表
    public Dictionary<string, Texture2D> dicCharacterMouthTex = new Dictionary<string, Texture2D>();
    public Dictionary<long, CharacterInfoBean> dicCharacterMouthInfo = new Dictionary<long, CharacterInfoBean>();

    //皮肤列表
    public Dictionary<string, Texture2D> dicCharacterSkinTex = new Dictionary<string, Texture2D>();
    public Dictionary<long, CharacterInfoBean> dicCharacterSkinInfo = new Dictionary<long, CharacterInfoBean>();

    //生物列表
    public Dictionary<long, CreatureInfoBean> dicCreatureInfo = new Dictionary<long, CreatureInfoBean>();
    public Dictionary<string, GameObject> dicCreatureModel = new Dictionary<string, GameObject>();

    //角色数据控制器
    protected CharacterInfoController controllerForCharacterInfo;
    //生物数据控制器
    protected CreatureInfoController controllerForCreatureInfo;

    //生物血条模型
    protected GameObject modelForLifeProgress;

    public void Awake()
    {
        controllerForCharacterInfo = new CharacterInfoController(this, this);
        controllerForCharacterInfo.GetAllCharacterInfoHairData(InitCharacterInfoHair);
        controllerForCharacterInfo.GetAllCharacterInfoEyeData(InitCharacterInfoEye);
        controllerForCharacterInfo.GetAllCharacterInfoMouthData(InitCharacterInfoMouth);
        controllerForCharacterInfo.GetAllCharacterInfoSkinData(InitCharacterInfoSkin);

        controllerForCreatureInfo = new CreatureInfoController(this,this);
        controllerForCreatureInfo.GetAllCreatureInfoData(InitCreatureInfo);
    }

    /// <summary>
    /// 初始化所有生物信息
    /// </summary>
    /// <param name="listData"></param>
    protected void InitCreatureInfo(List<CreatureInfoBean> listData)
    {
        InitData(dicCreatureInfo, listData);
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
    /// 初始化皮肤
    /// </summary>
    /// <param name="listData"></param>
    public void InitCharacterInfoSkin(List<CharacterInfoBean> listData)
    {
        InitData(dicCharacterSkinInfo, listData);
    }

    /// <summary>
    /// 获取生命条模型
    /// </summary>
    /// <returns></returns>
    public GameObject GetCreatureLifeProgressModel()
    {
        if (modelForLifeProgress == null) 
        {
            modelForLifeProgress = LoadAddressablesUtil.LoadAssetSync<GameObject>(pathCreatureLifeProgress);
        }
        return modelForLifeProgress;
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
    /// 获取生物信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CreatureInfoBean GetCreatureInfo(long id)
    {
        return GetDataById(id, dicCreatureInfo);
    }

    /// <summary>
    /// 获取角色发型信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<CharacterInfoBean> GetCharacterInfoHair(List<long> ids)
    {
        List<CharacterInfoBean> listData = new List<CharacterInfoBean>();
        for (int i = 0; i < ids.Count; i++)
        {
            CharacterInfoBean itemData = GetCharacterInfoHair(ids[i]);
            listData.Add(itemData);
        }
        return listData;
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
    /// 获取皮肤信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CharacterInfoBean GetCharacterInfoSkin(long id)
    {
        return GetDataById(id, dicCharacterSkinInfo);
    }

    /// <summary>
    /// 获取角色头发模型
    /// </summary>
    /// <param name="hairName"></param>
    /// <returns></returns>
    public void GetCharacterHairModel(string hairName, Action<GameObject> callBack)
    {
        GetModelForAddressables(dicCharacterHairModel, $"{pathHair}/{hairName}.fbx", callBack);
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

    /// <summary>
    /// 获取皮肤贴图
    /// </summary>
    /// <param name="skinName"></param>
    /// <param name="callBack"></param>
    public void GetCharacterSkinTex(string skinName, Action<Texture2D> callBack)
    {
        GetModelForAddressables(dicCharacterSkinTex, $"{pathSkin}/{skinName}.png", callBack);
    }

    /// <summary>
    /// 获取生物模组
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="callBack"></param>
    public void GetCreatureModel(string modelName, Action<GameObject> callBack)
    {
        GetModelForAddressables(dicCreatureModel, $"{pathCreature}/{modelName}.prefab", callBack);
    }

    #region 数据回调
    public void GetCharacterInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetCharacterInfoFail(string failMsg, Action action)
    {

    }

    public void GetCreatureInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetCreatureInfoFail(string failMsg, Action action)
    {
    }
    #endregion
}