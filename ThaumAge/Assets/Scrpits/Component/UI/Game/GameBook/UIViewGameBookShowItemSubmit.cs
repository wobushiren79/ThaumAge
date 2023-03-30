using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookShowItemSubmit : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BTSumit)
        {
            OnClickForSubmit();
        }
        ui_ViewGameBookShowItemSubmitDetails.ShowObj(false);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        SetSubmitState((int)bookModelDetailsInfo.id);
        SetUnlockItems(bookModelDetailsInfo.GetUnlockItems());
    }

    /// <summary>
    /// 设置待解锁道具
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetUnlockItems(List<ItemsArrayBean> listUnlockItems)
    {
        if (listUnlockItems.IsNull())
        {
            ui_Content.gameObject.SetActive(false);
            return;
        }
        ui_Content.gameObject.SetActive(true);
        ui_Content.DestroyAllChild(true,1);
        for (int i = 0; i < listUnlockItems.Count; i++)
        {
            GameObject objItem = Instantiate(ui_Content.gameObject, ui_ViewGameBookShowItemSubmitDetails.gameObject);
            objItem.ShowObj(true);
            UIViewGameBookShowItemSubmitDetails itemView = objItem.GetComponent<UIViewGameBookShowItemSubmitDetails>();
            itemView.SetData(bookModelDetailsInfo, listUnlockItems[i]);
        }
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
        AudioHandler.Instance.PlaySound(1);
        List<ItemsArrayBean> listUnlockItems = bookModelDetailsInfo.GetUnlockItems();
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        foreach (var itemUnlock in listUnlockItems)
        {
            bool hasEnoughItem = false;
            //只要其中一个满足就行
            foreach (var itemId in itemUnlock.itemIds)
            {
                hasEnoughItem = userData.HasEnoughItem(itemId, itemUnlock.itemNumber);
                if (hasEnoughItem)
                    break;
            }

            if (!hasEnoughItem)
            {
                UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.GetTextById(30003));
                return;
            }
        }
        //移除道具
        //foreach (var itemUnlock in listUnlockItems)
        //{
        //    userData.RemoveItem(itemUnlock.itemId, itemUnlock.number);
        //}
        //保存数据
        userData.userAchievement.UnlockBookModelDetails((int)bookModelDetailsInfo.id);
        //通知UI更新
        TriggerEvent(EventsInfo.UIGameBook_MapItemRefresh, bookModelDetailsInfo);
        TriggerEvent(EventsInfo.UIGameBook_RefreshLabels);
        //播放音效
        AudioHandler.Instance.PlaySound(901);
    }
}