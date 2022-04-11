using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookMapItem : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        SetPosition();
        SetIcon();
        SetItemState();
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    public void SetItemState()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        bool isUnlock = userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfo.id);
        //判断是否解锁
        if (isUnlock)
        {
            IconHandler.Instance.manager.GetUISpriteByName("ui_border_13",(sprite)=> 
            {
                ui_BG.sprite = sprite;
                ui_BG.color = Color.green;
                ui_Icon.material.SetFloat("_EffectAmount", 0);
            });
        }
        else
        {
            IconHandler.Instance.manager.GetUISpriteByName("ui_border_14", (sprite) =>
            {
                ui_BG.sprite = sprite;
                ui_BG.color = Color.white;
                ui_Icon.material.SetFloat("_EffectAmount", 1);
            });
        }
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    public void SetIcon()
    {
        IconHandler.Instance.GetIconSprite(bookModelDetailsInfo.icon_key,(sprite)=> 
        {
            ui_Icon.sprite = sprite;
        });
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    public void SetPosition()
    {
        string postionStr = bookModelDetailsInfo.map_position;
        float[] position = postionStr.SplitForArrayFloat(',');
        rectTransform.anchoredPosition = new Vector2(position[0], position[1]);
    }


    /// <summary>
    /// 按钮
    /// </summary>
    /// <param name="viewButton"></param>
    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BTSubmit)
        {
            OnClickForSubmit();
        }
    }

    /// <summary>
    /// 点击-提交
    /// </summary>
    protected void OnClickForSubmit()
    {
        TriggerEvent(EventsInfo.UIGameBook_MapItemChange, bookModelDetailsInfo);
    }
}
