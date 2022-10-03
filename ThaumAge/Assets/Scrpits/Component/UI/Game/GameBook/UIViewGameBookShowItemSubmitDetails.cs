using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookShowItemSubmitDetails : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    protected ItemsBean itemData;
    protected ItemsInfoBean unlockItemsInfo;
    public override void Awake()
    {
        base.Awake();
        ui_Icon.material = new Material(ui_Icon.material);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="unlockItemData"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo, ItemsBean itemData)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        this.itemData = itemData;
        unlockItemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        //设置图标
        SetIcon(unlockItemsInfo.icon_key);
        //设置数量
        int curItemNumber = userData.CheckItemNumber(itemData.itemId);
        SetNumber(curItemNumber, itemData.number);

        //添加点位颜色
        bool isUnlockSelf = userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfo.id);
        if (isUnlockSelf)
        {
            SetStatus(2);
        }
        else
        {
            if (userData.HasEnoughItem(itemData.itemId, itemData.number))
            {
                SetStatus(1);
            }
            else
            {
                SetStatus(0);
            }
        }

        //设置展示信息
        ui_InfoShow.SetItemData(itemData);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="iconKey"></param>
    public void SetIcon(string iconKey)
    {
        IconHandler.Instance.manager.GetItemsSpriteByName(iconKey, (spIcon) =>
        {
            ui_Icon.sprite = spIcon;
        });
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="currentNumber"></param>
    /// <param name="maxNumber"></param>
    public void SetNumber(int currentNumber, int maxNumber)
    {
        ui_Number.text = $"{currentNumber}/{maxNumber}";
    }


    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="type">0没有达成条件  1达成条件  2已经完成</param>
    public void SetStatus(int type)
    {
        Image backgroundIv = rectTransform.GetComponent<Image>();
        switch (type)
        {
            case 0:
                ui_Number.ShowObj(true);
                ui_Number.color = Color.white;
                backgroundIv.color = Color.white;
                ui_Icon.materialForRendering.SetFloat("_EffectAmount", 1);
                break;
            case 1:
                ui_Number.ShowObj(true);
                ui_Number.color = Color.green;
                backgroundIv.color = Color.green;
                ui_Icon.materialForRendering.SetFloat("_EffectAmount", 0);
                break;
            case 2:
                ui_Number.ShowObj(false);
                backgroundIv.color = Color.green;
                ui_Icon.materialForRendering.SetFloat("_EffectAmount", 0);
                break;
        }
    }
}