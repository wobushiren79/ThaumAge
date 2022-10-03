using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIViewGameBookMapItem : BaseUIView
{
    [HideInInspector]
    public BookModelDetailsInfoBean bookModelDetailsInfo;

    protected UIViewGameBookContentMap uiGameBookContentMap;

    protected List<LineView> listLine = new List<LineView>();
    public override void Awake()
    {
        base.Awake();
        Material matIcon = new Material(ui_Icon.material);
        ui_Icon.material = matIcon;
        ui_LineModel.ShowObj(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void OnDestroy()
    {
        for (int i = 0; i < listLine.Count; i++)
        {
            Destroy(listLine[i]);
        }
        listLine.Clear();
        base.OnDestroy();
    }

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
        CreatePreShowLine();
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
                ui_Icon.materialForRendering.SetFloat("_EffectAmount", 0);
            });
        }
        else
        {
            IconHandler.Instance.manager.GetUISpriteByName("ui_border_14", (sprite) =>
            {
                ui_BG.sprite = sprite;
                ui_BG.color = Color.white;
                ui_Icon.materialForRendering.SetFloat("_EffectAmount", 1);
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
        if (bookModelDetailsInfo == null)
            return;
        if (bookModelDetailsInfo.show_pre_line == 0)
            return;
        int[] preShowIds = bookModelDetailsInfo.GetPreShowIds();
        for (int i = 0; i < preShowIds.Length; i++)
        {
            int itemPreShowId = preShowIds[i];
            //��������
            GameObject objLine = Instantiate(transform.parent.gameObject, ui_LineModel.gameObject);
            objLine.ShowObj(true);
            objLine.transform.SetAsFirstSibling();
            LineView lineViewItem = objLine.GetComponent<LineView>();
            //�����������
            lineViewItem.listPosition.Clear();
            lineViewItem.listPositionColor.Clear();

            //������������
            lineViewItem.lineThickness = 3;
            lineViewItem.linePositionDirection = 0;

            if (uiGameBookContentMap.dicBookModelInfoDetails.TryGetValue(itemPreShowId, out var itemPreShowData))
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
                        lineViewItem.listPositionColor.Add(new Color(0, 0, 0, 0));
                    }
                }
                //���ǰ��û�н���
                else
                {
                    lineViewItem.listPositionColor.Add(new Color(0, 0, 0, 0));
                    if (isUnlockSelf)
                    {
                        lineViewItem.listPositionColor.Add(Color.green);
                    }
                    //�������û�н��� ����������
                    else
                    {
                        lineViewItem.listPositionColor.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            listLine.Add(lineViewItem);
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
        //������Ч
        AudioHandler.Instance.PlaySound(801);
    }

}
