using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public partial class UIViewGameBookContentMap : BaseUIView
{
    [HideInInspector]
    public Dictionary<int, BookModelDetailsInfoBean> dicBookModelInfoDetails;
    [HideInInspector]
    public BookModelInfoBean bookModelInfo;
    [HideInInspector]
    public float uiSize = 2;
    [HideInInspector]
    public List<UIViewGameBookMapItem> listMapItem;

    public override void Awake()
    {
        base.Awake();
        ui_ViewGameBookMapItem.gameObject.SetActive(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemChange, EventForMapItemChange);
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemRefresh, EventForMapItemRefresh);
    }

    public void SetData(BookModelInfoBean bookModelInfo)
    {
        this.bookModelInfo = bookModelInfo;
        SetContentBG();
        SetContentSizePosition();
        InitMapData();

        //默认打开清空内容
        TriggerEvent(EventsInfo.UIGameBook_MapItemClean);
        ui_Select.ShowObj(false);
    }

    /// <summary>
    /// 按钮输入监听
    /// </summary>
    /// <param name="inputType"></param>
    public override void OnInputActionForStarted(InputActionUIEnum inputType, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        switch (inputType)
        {
            //滚轮滑动
            case InputActionUIEnum.ScrollWheel:
                Vector2 scrollSize = callback.ReadValue<Vector2>();
                ScrollContentSize(scrollSize.normalized);
                break;
        }
    }

    /// <summary>
    /// 初始化地图数据
    /// </summary>
    public void InitMapData()
    {
        if (bookModelInfo == null)
            return;
        if (dicBookModelInfoDetails == null)
            dicBookModelInfoDetails = new Dictionary<int, BookModelDetailsInfoBean>();
        if (listMapItem == null)
            listMapItem = new List<UIViewGameBookMapItem>();
        //清除数据
        dicBookModelInfoDetails.Clear();
        foreach (var itemMap in listMapItem)
        {
            DestroyImmediate(itemMap.gameObject);
        }
        listMapItem.Clear();
        //获取数据
        var listBookModelInfoDetails = GameInfoHandler.Instance.manager.GetBookModelDetailsById(bookModelInfo.id);
        for (int i = 0; i < listBookModelInfoDetails.Count; i++)
        {
            var itemData = listBookModelInfoDetails[i];
            dicBookModelInfoDetails.Add(itemData.id, itemData);
        }

        if (listBookModelInfoDetails == null || listBookModelInfoDetails.Count == 0)
            return;

        for (int i = 0; i < listBookModelInfoDetails.Count; i++)
        {
            //生成地图的点位
            BookModelDetailsInfoBean itemData = listBookModelInfoDetails[i];
            bool isPreShow = itemData.CheckPreShow();
            if (!isPreShow)
                continue;
            GameObject objItem = Instantiate(ui_ContentBG.gameObject, ui_ViewGameBookMapItem.gameObject);
            UIViewGameBookMapItem mapItem = objItem.GetComponent<UIViewGameBookMapItem>();
            mapItem.SetData(itemData, this);
            listMapItem.Add(mapItem);

            objItem.transform.localScale = Vector3.one;
            objItem.transform.DOScale(0.8f,0.25f).From().SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 设置背景
    /// </summary>
    public void SetContentBG()
    {
        IconHandler.Instance.manager.GetUISpriteByName(bookModelInfo.background, (iconSprite) =>
        {
            ui_ContentBG.sprite = iconSprite;
        });
    }

    /// <summary>
    /// 初始化位置和大小
    /// </summary>
    public void SetContentSizePosition()
    {
        ui_ViewGameBookContentMap.normalizedPosition = new Vector2(0.5f, 0.5f);
        uiSize = 2;
        ui_ContentBG.rectTransform.localScale = Vector3.one * uiSize;
    }

    /// <summary>
    /// 滚动缩放大小
    /// </summary>
    public void ScrollContentSize(Vector2 normalized)
    {
        uiSize += normalized.y * 0.2f;
        if (uiSize < 1)
            uiSize = 1;
        if (uiSize > 4)
            uiSize = 4;
        ui_ContentBG.rectTransform.localScale = Vector3.one * uiSize;
    }


    /// <summary>
    /// 事件-地图item选择改变
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    protected void EventForMapItemChange(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        ui_Select.ShowObj(true);
        ui_Select.transform.SetAsLastSibling();
        ui_Select.anchoredPosition = bookModelDetailsInfo.GetMapPosition();

        foreach (var itemView in listMapItem) 
        {
            if (itemView.bookModelDetailsInfo == bookModelDetailsInfo)
            {
                itemView.transform.localScale = Vector3.one;
                itemView.transform.DOScale(0.8f, 0.25f).From().SetEase(Ease.OutBack);
            }
        }
    }

    /// <summary>
    /// 事件-地图item刷新
    /// </summary>
    protected void EventForMapItemRefresh(BookModelDetailsInfoBean bookModelDetailsInfo)
    {
        InitMapData();
        ui_Select.transform.SetAsLastSibling();
    }
}

