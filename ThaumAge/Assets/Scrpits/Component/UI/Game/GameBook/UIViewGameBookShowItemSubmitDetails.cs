using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookShowItemSubmitDetails : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    protected ItemsArrayBean itemData;

    public override void Awake()
    {
        base.Awake();
        ui_Icon.material = new Material(ui_Icon.material);
    }

    public override void OnDestroy()
    {
        StopAllAnim();
        base.OnDestroy();
    }
    public void StopAllAnim()
    {
        rectTransform.DOKill();
        StopAllCoroutines();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="unlockItemData"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo, ItemsArrayBean itemData)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        this.itemData = itemData;

        StopAllAnim();

        if (itemData.itemIds.Length == 1)
        {
            ChangeItem(itemData.itemIds[0]);
        }
        else
        {
            StopAllCoroutines();
            AnimForChange(itemData.itemIds, 0);
        }
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

    /// <summary>
    /// 改变道具
    /// </summary>
    /// <param name="itemId"></param>
    public void ChangeItem(long itemId)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsInfoBean unlockItemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        //设置图标
        SetIcon(unlockItemsInfo.icon_key);
        //设置展示信息
        ui_InfoShow.SetItemData(new ItemsBean(itemId));
        //设置数量
        int curItemNumber = userData.CheckItemNumber(itemId);
        SetNumber(curItemNumber, itemData.itemNumber);

        //添加点位颜色
        bool isUnlockSelf = userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfo.id);
        if (isUnlockSelf)
        {
            SetStatus(2);
        }
        else
        {
            if (userData.HasEnoughItem(itemId, itemData.itemNumber))
            {
                SetStatus(1);
            }
            else
            {
                SetStatus(0);
            }
        }
    }

    /// <summary>
    /// 改变道具动画，用于可替换道具
    /// </summary>
    public void AnimForChange(long[] listItemsId, int startIndex)
    {
        ChangeItem(listItemsId[startIndex]);
        this.WaitExecuteSeconds(2, () =>
        {
            startIndex++;
            if (startIndex >= listItemsId.Length)
                startIndex = 0;
            ChangeItem(listItemsId[startIndex]);
            AnimForChange(listItemsId, startIndex);
        });
    }
}