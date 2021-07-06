using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIView : BaseMonoBehaviour
{
    protected RectTransform rectTransform;
    //原始UI大小
    protected Vector2 uiSizeOriginal;

    public virtual void Awake()
    {
        AutoLinkUI();
        InitButtons();
        rectTransform = ((RectTransform)transform);
        uiSizeOriginal = rectTransform.sizeDelta;
    }

    protected virtual void OnEnable()
    {
        RefreshUI();
    }

    /// <summary>
    /// 刷新UI大小
    /// </summary>
    public virtual void RefreshUI()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        ChangeUISize(gameConfig.uiSize);
    }

    /// <summary>
    /// 修改UI大小
    /// </summary>
    /// <param name="size"></param>
    public virtual void ChangeUISize(float size)
    {
        if (rectTransform != null)
            rectTransform.sizeDelta = new Vector2(uiSizeOriginal.x * size, uiSizeOriginal.y * size);
    }

    /// <summary>
    /// 初始化所有按钮点击事件
    /// </summary>
    public void InitButtons()
    {
        Button[] buttonArray = gameObject.GetComponentsInChildren<Button>();
        if (CheckUtil.ArrayIsNull(buttonArray))
            return;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            Button itemButton = buttonArray[i];
            itemButton.onClick.AddListener(() => { OnClickForButton(itemButton); });
        }
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    public virtual void OnClickForButton(Button viewButton)
    {

    }
}