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

    }

    /// <summary>
    /// 初始化所有按钮点击事件
    /// </summary>
    public void InitButtons()
    {
        Button[] buttonArray = gameObject.GetComponentsInChildren<Button>();
        if (buttonArray.IsNull())
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