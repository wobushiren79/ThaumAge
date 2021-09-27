using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIComponent : BaseMonoBehaviour
{
    //UI管理
    public BaseUIManager uiManager;
    //备注数据
    public string remarkData;

    public virtual void Awake()
    {
        if (uiManager == null)
            uiManager = GetComponentInParent<BaseUIManager>();
        AutoLinkUI();
        InitButtons();
    }

    /// <summary>
    /// 开启UI
    /// </summary>
    public virtual void OpenUI()
    {
        if (this.gameObject.activeSelf)
            return;
        this.gameObject.SetActive(true);
        RefreshUI();
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public virtual void CloseUI()
    {
        StopAllCoroutines();
        if (!this.gameObject.activeSelf)
            return;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public virtual void RefreshUI()
    {

    }

    /// <summary>
    /// 设置备用数据
    /// </summary>
    /// <param name="remarkData"></param>
    public virtual void SetRemarkData(string remarkData)
    {
        this.remarkData = remarkData;
    }

    public T GetUIManager<T>() where T : BaseUIManager
    {
        return uiManager as T;
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
