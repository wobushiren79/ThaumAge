using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookShowItemSubmit : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if(viewButton == ui_BTSumit)
        {
            OnClickForSubmit();
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        SetSubmitState(bookModelDetailsInfo.id);
        SetUnlockItems(bookModelDetailsInfo.unlock_items);
    }

    /// <summary>
    /// 设置待解锁道具
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetUnlockItems(string unlockItemsData)
    {
        if (unlockItemsData.IsNull())
        {
            ui_ItemList.gameObject.SetActive(false);
            return;
        }
        ui_ItemList.gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置提交状态
    /// </summary>
    public void SetSubmitState(int bookModelDetailsInfoId)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        //检测该模块是否解锁
        if (userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfoId))
        {
            ui_BTComplete.ShowObj(true);
            ui_BTSumit.ShowObj(false);
        }
        else
        {
            ui_BTComplete.ShowObj(false);
            ui_BTSumit.ShowObj(true);
        }
    }

    /// <summary>
    /// 点击 提交
    /// </summary>
    public void OnClickForSubmit()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userAchievement.UnlockBookModelDetails(bookModelDetailsInfo.id);

        TriggerEvent(EventsInfo.UIGameBook_MapItemRefresh, bookModelDetailsInfo);
        AudioHandler.Instance.PlaySound(901);
    }
}