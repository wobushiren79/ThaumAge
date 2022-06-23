using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterEquip : CharacterBase
{
    //衣服容器
    public GameObject objClothesContainer;

    public CharacterEquip(CreatureCptCharacter character) : base(character)
    {
        if (character.characterHead == null)
        {
            LogUtil.LogError($"初始化角色失败，{character.gameObject.name}的角色 缺少 Clothes 部件");
            return;
        }
        objClothesContainer = character.characterClothes;
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
    }

    /// <summary>
    /// 改变装备
    /// </summary>
    /// <param name="equipType"></param>
    /// <param name="clothesId"></param>
    public void ChangeEquip(EquipTypeEnum equipType, long clothesId, Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
    {
        switch (equipType)
        {
            case EquipTypeEnum.Hats:
                return;//帽子
            case EquipTypeEnum.Clothes:
                this.characterData.clothesId = clothesId;
                ChangeEquipDetails(clothesId, objClothesContainer, callBack, callBackModelRemark);
                return;//衣服
            case EquipTypeEnum.Gloves:
                return;//手套
            case EquipTypeEnum.Shoes:
                return;//鞋子
            case EquipTypeEnum.Trousers:
                return;//裤子
            case EquipTypeEnum.Headwear:
                return;//头饰
            case EquipTypeEnum.LeftRing:
                return;//戒指
            case EquipTypeEnum.RightRing:
                return;//戒指
            case EquipTypeEnum.Cape:
                return;//披风
        }
    }

    protected void ChangeEquipDetails(long equipId, GameObject objEquipContainer, Action<GameObject> callBack = null, Action<IList<GameObject>> callBackModelRemark = null)
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
                    callBackModelRemark?.Invoke(listItemsRemarkObj);
                });
        }
    }
}