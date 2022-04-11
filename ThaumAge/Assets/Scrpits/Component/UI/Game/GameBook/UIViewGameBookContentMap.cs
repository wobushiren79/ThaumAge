using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookContentMap : BaseUIView
{
    protected List<BookModelDetailsInfoBean> listBookModelInfoDetails;
    protected BookModelInfoBean bookModelInfo;
    protected float uiSize = 2;

    public override void Awake()
    {
        base.Awake();
        ui_ViewGameBookMapItem.gameObject.SetActive(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RegisterEvent<BookModelDetailsInfoBean>(EventsInfo.UIGameBook_MapItemChange, EventForMapItemChange);
    }

    public void SetData(BookModelInfoBean bookModelInfo)
    {
        this.bookModelInfo = bookModelInfo;
        SetContentBG();
        SetContentSizePosition();
        InitMapData();
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
        listBookModelInfoDetails = GameInfoHandler.Instance.manager.GetBookModelDetailsById(bookModelInfo.id);
        ui_ContentBG.transform.DestroyAllChild(true, 1);
        if (listBookModelInfoDetails == null || listBookModelInfoDetails.Count == 0)
            return;
        for (int i = 0; i < listBookModelInfoDetails.Count; i++)
        {
            BookModelDetailsInfoBean itemData = listBookModelInfoDetails[i];
            GameObject objItem = Instantiate(ui_ContentBG.gameObject, ui_ViewGameBookMapItem.gameObject);
            UIViewGameBookMapItem mapItem = objItem.GetComponent<UIViewGameBookMapItem>();
            mapItem.SetData(itemData);
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
    /// 事件-地图item改变
    /// </summary>
    /// <param name="bookModelDetailsInfo"></param>
    protected void EventForMapItemChange(BookModelDetailsInfoBean bookModelDetailsInfo)
    {

    }
}

