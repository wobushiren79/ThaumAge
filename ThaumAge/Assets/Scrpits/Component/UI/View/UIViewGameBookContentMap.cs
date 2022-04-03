using UnityEditor;
using UnityEngine;

public partial class UIViewGameBookContentMap : BaseUIView
{
    protected BookModelInfoBean bookModelInfo;
    protected float uiSize = 2;
    public void SetData(BookModelInfoBean bookModelInfo)
    {
        this.bookModelInfo = bookModelInfo;
        SetContentBG();
        SetContentSizePosition();
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
}

