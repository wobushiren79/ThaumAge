using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookMapItem : BaseUIView
{
    protected BookModelDetailsInfoBean bookModelDetailsInfo;
    protected UIViewGameBookContentMap uiGameBookContentMap;

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    public void SetData(BookModelDetailsInfoBean bookModelDetailsInfo, UIViewGameBookContentMap uiGameBookContentMap)
    {
        this.bookModelDetailsInfo = bookModelDetailsInfo;
        this.uiGameBookContentMap = uiGameBookContentMap;
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
            IconHandler.Instance.manager.GetUISpriteByName("ui_border_13", (sprite) =>
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
        IconHandler.Instance.GetIconSprite(bookModelDetailsInfo.icon_key, (sprite) =>
         {
             ui_Icon.sprite = sprite;
         });
    }

    /// <summary>
    /// ����λ��
    /// </summary>
    public void SetPosition()
    {
        rectTransform.anchoredPosition = bookModelDetailsInfo.GetMapPosition();
    }

    /// <summary>
    /// ����ǰ��������
    /// </summary>
    public void CreatePreShowLine()
    {
        int[] preShowIds = bookModelDetailsInfo.GetPreShowIds();
        for (int i = 0; i < preShowIds.Length; i++)
        {
            int itemPreShowId = preShowIds[i];
            //��������
            GameObject objLine = Instantiate(transform.parent.gameObject, null);
            LineView lineViewItem = objLine.GetComponent<LineView>();
            //�����������
            lineViewItem.listPosition.Clear();
            lineViewItem.listPositionColor.Clear();

            //������������
            lineViewItem.lineThickness = 1;
            lineViewItem.linePositionDirection = 0;

            if (uiGameBookContentMap.dicBookModelInfoDetails.TryGetValue(itemPreShowId,out var itemPreShowData))
            {
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();

                Vector2 startPosition = itemPreShowData.GetMapPosition();
                Vector2 endPosition = bookModelDetailsInfo.GetMapPosition();

                lineViewItem.listPosition.Add(startPosition);
                lineViewItem.listPosition.Add(endPosition);

                //��ӵ�λ��ɫ
                bool isUnlockSelf = userData.userAchievement.CheckUnlockBookModelDetails(bookModelDetailsInfo.id);
                bool isUnlockPreShow = userData.userAchievement.CheckUnlockBookModelDetails(itemPreShowData.id);
                //���ǰ���Ѿ�����
                if (isUnlockPreShow)
                {
                    lineViewItem.listPositionColor.Add(Color.green);
                    //��������Ѿ����� ������ȫ��
                    if (isUnlockSelf)
                    {
                        lineViewItem.listPositionColor.Add(Color.green);
                    }
                    //�������û�н��� ����������
                    else
                    {
                        lineViewItem.listPositionColor.Add(Color.green / 2f);
                    }
                }
                //���ǰ��û�н���
                else
                {
                    lineViewItem.listPositionColor.Add(Color.gray);
                    lineViewItem.listPositionColor.Add(Color.gray / 2f);
                }
            }
        }
    }

    /// <summary>
    /// ��ť
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
    /// ���-�ύ
    /// </summary>
    protected void OnClickForSubmit()
    {
        TriggerEvent(EventsInfo.UIGameBook_MapItemChange, bookModelDetailsInfo);
    }
}
