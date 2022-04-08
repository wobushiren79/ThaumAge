using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIViewGameBookMapItem : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;

    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        SetPosition();
        SetIcon();
        SetItemState();
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public void SetItemState()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        bool isUnlock = userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfo.id);
        //�ж��Ƿ����
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
    /// ����ͼ��
    /// </summary>
    public void SetIcon()
    {
        IconHandler.Instance.GetIconSprite(bookModelDetailsInfo.icon_key,(sprite)=> 
        {
            ui_Icon.sprite = sprite;
        });
    }

    /// <summary>
    /// ����λ��
    /// </summary>
    public void SetPosition()
    {
        string postionStr = bookModelDetailsInfo.map_position;
        float[] position = postionStr.SplitForArrayFloat(',');
        rectTransform.anchoredPosition = new Vector2(position[0], position[1]);
    }
}
