using System;
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

        ChangeClothes(this.characterData.clothesId);
    }

    /// <summary>
    /// 改变装备
    /// </summary>
    /// <param name="equipType"></param>
    /// <param name="clothesId"></param>
    public void ChangeEquip(EquipTypeEnum equipType, long clothesId, Action<GameObject> callBack = null)
    {
        switch (equipType)
        {
            case EquipTypeEnum.Hats:
                return;//帽子
            case EquipTypeEnum.Clothes:
                ChangeClothes(clothesId, callBack);
                return;//衣服
            case EquipTypeEnum.Gloves:
                return;//手套
            case EquipTypeEnum.Shoes:
                return;//鞋子
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

    /// <summary>
    /// 改变衣服
    /// </summary>
    /// <param name="clothesId"></param>
    public void ChangeClothes(long clothesId, Action<GameObject> callBack = null)
    {
        this.characterData.clothesId = clothesId;
        CptUtil.RemoveChild(objClothesContainer.transform);
        if (clothesId == 0)
        {
            //没有衣服
            return;
        }
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(clothesId);
        if (itemsInfo == null)
        {
            LogUtil.LogError($"查询道具数据失败，没有ID为 {clothesId} 的服装数据");
        }
        else
        {
            ItemsHandler.Instance.manager.GetItemsObjById(clothesId, (itemsObj) =>
             {
                 if (itemsObj == null)
                 {
                     LogUtil.LogError($"查询道具模型失败，没有ID为 {clothesId} 的道具模型");
                 }
                 else
                 {
                     GameObject objModel = ItemsHandler.Instance.Instantiate(objClothesContainer, itemsObj);
                     objModel.transform.localPosition = Vector3.zero;
                     objModel.transform.localEulerAngles = Vector3.zero;

                     ItemsHandler.Instance.manager.GetItemsTexById(itemsInfo.id, (itemTex) =>
                     {
                         if (objModel == null)
                             return;
                         MeshRenderer meshRebderer = objModel.GetComponent<MeshRenderer>();
                         meshRebderer.material.mainTexture = itemTex;
                     });

                     callBack?.Invoke(objModel);
                 }
             });
        }
    }
}