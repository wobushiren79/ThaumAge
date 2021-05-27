using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

}
