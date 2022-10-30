using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterEquip : CharacterBase
{
    //衣服容器
    public GameObject objClothesContainer;
    //帽子容器
    public GameObject objHatContainer;
    //鞋子容器
    public GameObject objShoesRContainer;
    public GameObject objShoesLContainer;
    //衣服袖子容器
    public GameObject objClothesRContainer;
    public GameObject objClothesLContainer;
    //裤子容器
    public GameObject objTrousersLContainer;
    public GameObject objTrousersRContainer;

    public bool isShowHead = true;

    public CharacterEquip(CreatureCptCharacter character) : base(character)
    {
        if (character.characterHead == null)
        {
            LogUtil.LogError($"初始化角色失败，{character.gameObject.name}的角色 缺少 Clothes 部件");
            return;
        }

        objClothesContainer = character.characterClothes;
        objHatContainer = character.characterHat;

        objShoesRContainer = character.characterShoesR;
        objShoesLContainer = character.characterShoesL;

        objClothesRContainer = character.characterClothesRight;
        objClothesLContainer = character.characterClothesLeft;

        objTrousersLContainer = character.characterTrousersL;
        objTrousersRContainer = character.characterTrousersR;
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterData"></param>
    public override void SetCharacterData(CharacterBean characterData)
    {
        base.SetCharacterData(characterData);
        //初始化设置衣服
        CharacterEquipBean characterEquipData = characterData.GetCharacterEquip();
        ChangeEquip(EquipTypeEnum.Hats, characterEquipData.hats);
        ChangeEquip(EquipTypeEnum.Clothes, characterEquipData.clothes);
        ChangeEquip(EquipTypeEnum.Shoes, characterEquipData.shoes);
        ChangeEquip(EquipTypeEnum.Trousers, characterEquipData.trousers);
    }

    /// <summary>
    /// 改变装备
    /// </summary>
    public void ChangeEquip(EquipTypeEnum equipType, ItemsBean equipData, Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        long equipId = 0;
        if (equipData != null)
        {
            equipId = equipData.itemId;
        }
        CharacterEquipBean characterEquipData = characterData.GetCharacterEquip();
        switch (equipType)
        {
            case EquipTypeEnum.Hats://帽子
                characterEquipData.hats = equipData;
                Action<GameObject> callBackForHat = (obj) =>
                {
                    //如果是玩家 再刷新一次头部显示
                    if (character.creatureData.GetCreatureType() == CreatureTypeEnum.Player)
                    {
                        callBack?.Invoke(obj);
                        Player player = GameHandler.Instance.manager.player;
                        player.SetHeadShow();
                    }
                };
                ChangeEquipDetails(equipId, objHatContainer, callBack : callBackForHat, callBackModelRemark : callBackModelRemark);

                return;
            case EquipTypeEnum.Clothes://衣服
                characterEquipData.clothes = equipData;
                ChangeEquipDetails(equipId, objClothesContainer, new List<GameObject>() { objClothesRContainer, objClothesLContainer }, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Gloves://手套
                return;
            case EquipTypeEnum.Shoes://鞋子
                characterEquipData.shoes = equipData;
                ChangeEquipDetails(equipId, objShoesLContainer, new List<GameObject>() { objShoesRContainer }, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Trousers://裤子
                characterEquipData.trousers = equipData;
                ChangeEquipDetails(equipId, objTrousersLContainer, new List<GameObject>() { objTrousersRContainer }, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Headwear://头饰
                return;
            case EquipTypeEnum.LeftRing://戒指
                return;
            case EquipTypeEnum.RightRing://戒指
                return;
            case EquipTypeEnum.Cape://披风
                return;
        }
    }
    public void ChangeEquip(EquipTypeEnum equipType, long equipId, Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        ItemsBean itemsData = new ItemsBean(equipId,1);
        ChangeEquip(equipType, itemsData, callBack, callBackModelRemark);
    }

    protected void ChangeEquipDetails(long equipId, GameObject objEquipContainer, List<GameObject> objEquipRemarkContainer = null,
        Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        //清空容器
        CptUtil.RemoveChild(objEquipContainer.transform);
        if (objEquipRemarkContainer != null)
            for (int i = 0; i < objEquipRemarkContainer.Count; i++)
            {
                var itemContainer = objEquipRemarkContainer[i];
                CptUtil.RemoveChild(itemContainer.transform);
            }
        if (equipId == 0)
        {
            //没有装备
            return;
        }
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(equipId);
        if (itemsInfo == null)
        {
            LogUtil.LogError($"查询道具数据失败，没有ID为 {equipId} 的装备数据");
        }
        else
        {
            ItemsHandler.Instance.manager.GetItemsObjById(equipId,
                (itemsObj) =>
                {
                    if (itemsObj == null)
                    {
                        LogUtil.LogError($"查询道具模型失败，没有ID为 {equipId} 的道具模型");
                        return;
                    }
                    GameObject objModel = ItemsHandler.Instance.Instantiate(objEquipContainer, itemsObj);
                    objModel.transform.localPosition = Vector3.zero;
                    objModel.transform.localEulerAngles = Vector3.zero;
                    callBack?.Invoke(objModel);
                },
                (listItemsRemarkObj) =>
                {
                    if (listItemsRemarkObj == null || listItemsRemarkObj.Count == 0)
                        return;
                    if (objEquipRemarkContainer == null)
                        return;
                    for (int i = 0; i < objEquipRemarkContainer.Count; i++)
                    {
                        GameObject objItemContainer = objEquipRemarkContainer[i];
                        int indexRemark = 0;
                        if (listItemsRemarkObj.Count > i)
                            indexRemark = i;
                        else
                            indexRemark = listItemsRemarkObj.Count - 1;

                        GameObject objEquipModel = listItemsRemarkObj[indexRemark];
                        GameObject objModel = ItemsHandler.Instance.Instantiate(objItemContainer, objEquipModel);
                        objModel.transform.localPosition = Vector3.zero;
                        objModel.transform.localEulerAngles = Vector3.zero;
                    }
                    callBackModelRemark?.Invoke(listItemsRemarkObj);
                });
        }
    }
}