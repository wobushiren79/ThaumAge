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
    }

    /// <summary>
    /// 设置角色数据
    /// </summary>
    /// <param name="characterData"></param>
    public override void SetCharacterData(CharacterBean characterData)
    {
        base.SetCharacterData(characterData);
        //初始化设置衣服
        ChangeEquip(EquipTypeEnum.Clothes, this.characterData.clothesId);
        ChangeEquip(EquipTypeEnum.Shoes, this.characterData.shoesId);
    }

    /// <summary>
    /// 改变装备
    /// </summary>
    /// <param name="equipType"></param>
    /// <param name="clothesId"></param>
    public void ChangeEquip(EquipTypeEnum equipType, long equipId, Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        switch (equipType)
        {
            case EquipTypeEnum.Hats://帽子
                this.characterData.headId = equipId;
                ChangeEquipDetails(equipId, objHatContainer, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Clothes://衣服
                this.characterData.clothesId = equipId;
                ChangeEquipDetails(equipId, objClothesContainer, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Gloves://手套
                return;
            case EquipTypeEnum.Shoes://鞋子
                this.characterData.shoesId = equipId;
                ChangeEquipDetails(equipId, objShoesLContainer, new List<GameObject>() { objShoesRContainer }, callBack: callBack, callBackModelRemark: callBackModelRemark);
                return;
            case EquipTypeEnum.Trousers://裤子
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

    protected void ChangeEquipDetails(long equipId, GameObject objEquipContainer, List<GameObject> objEquipRemarkContainer = null,
        Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        CptUtil.RemoveChild(objEquipContainer.transform);
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
                        if (listItemsRemarkObj.Count > i)
                        {
                            GameObject objEquipModel = listItemsRemarkObj[i];
                            GameObject objModel = ItemsHandler.Instance.Instantiate(objItemContainer, objEquipModel);
                            objModel.transform.localPosition = Vector3.zero;
                            objModel.transform.localEulerAngles = Vector3.zero;
                        }
                    }
                    callBackModelRemark?.Invoke(listItemsRemarkObj);
                });
        }
    }
}