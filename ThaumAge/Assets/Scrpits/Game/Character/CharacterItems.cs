using UnityEditor;
using UnityEngine;

public class CharacterItems : CharacterBase
{
    public GameObject objRightHand;
    public GameObject objLeftHand;

    public ItemCptHold itemHoldRight;

    public CharacterItems(CreatureCptCharacter character) : base(character)
    {
        objRightHand = character.characterRightHand;
        itemHoldRight = objRightHand.GetComponentInChildren<ItemCptHold>();
    }

    /// <summary>
    /// 改变右手握住的东西
    /// </summary>
    public void ChangeRightHandItem(long itemId)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        if (itemsInfo == null || itemsInfo.id == 0)
        {
            itemHoldRight.ShowObj(false);
            return;
        }

        itemHoldRight?.SetItem(itemsInfo);
        ItemsTypeEnum itemsType = itemsInfo.GetItemsType();
        switch (itemsType)
        {
            case ItemsTypeEnum.Hoe:
                itemHoldRight.transform.localEulerAngles = new Vector3(90f, -30f, 0f);
                break;
            default:
                itemHoldRight.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                break;
        }
        itemHoldRight.ShowObj(true);
    }
}