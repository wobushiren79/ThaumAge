using UnityEditor;
using UnityEngine;

public class CharacterItems : CharacterBase
{
    public GameObject objRightHand;
    public GameObject objLeftHand;

    public ItemCptHold itemHoldRight;
    //当前数据
    protected ItemsBean curItemsData;

    public CharacterItems(CreatureCptCharacter character) : base(character)
    {
        objRightHand = character.characterRightHand;
        itemHoldRight = objRightHand.GetComponentInChildren<ItemCptHold>();
    }

    /// <summary>
    /// 改变右手握住的东西
    /// </summary>
    public void ChangeRightHandItem(ItemsBean itemsData)
    {
        if (itemsData == null)
        {
            itemHoldRight.ShowObj(false);
            return;
        }

        if (curItemsData == itemsData)
        {
            if (itemsData.itemId == 0)
            {
                itemHoldRight.ShowObj(false);
            }
            return;
        }

        this.curItemsData = itemsData;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
        if (itemsInfo == null || itemsInfo.id == 0)
        {
            itemHoldRight.ShowObj(false);
            return;
        }

        itemHoldRight?.SetItem(itemsData, itemsInfo);

        if (itemsInfo.GetHoldData(out Vector3 holdRotate,out Vector3 holdPosition))
        {
            itemHoldRight.transform.localEulerAngles = holdRotate;
            itemHoldRight.transform.localPosition = holdPosition;
        }
        else
        {
            ItemsTypeEnum itemsType = itemsInfo.GetItemsType();
            switch (itemsType)
            {
                case ItemsTypeEnum.Hoe:
                case ItemsTypeEnum.Pickaxe:
                case ItemsTypeEnum.Axe:
                case ItemsTypeEnum.Shovel:

                case ItemsTypeEnum.Sword:
                case ItemsTypeEnum.Knife:
                    itemHoldRight.transform.localPosition = new Vector3(0, 0, 0.25f);
                    itemHoldRight.transform.localEulerAngles = new Vector3(90f, -40f, 0f);
                    break;
                case ItemsTypeEnum.Bow:
                    itemHoldRight.transform.localPosition = new Vector3(0, 0, 0);
                    itemHoldRight.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
                    break;
                default:
                    itemHoldRight.transform.localPosition = new Vector3(0, 0, 0.25f);
                    itemHoldRight.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                    break;
            }
        }

        itemHoldRight.ShowObj(true);
    }
}