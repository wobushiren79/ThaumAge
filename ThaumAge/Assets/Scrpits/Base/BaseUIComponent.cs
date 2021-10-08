using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

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
        RegisterButtons();
        RegisterInputAction();
    }

    public virtual void OnDestroy()
    {
        UnRegisterInputAction();
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
    public void RegisterButtons()
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
    /// 初始化所有输入事件
    /// </summary>
    public void RegisterInputAction()
    {
        Dictionary<InputActionUIEnum, InputAction> dicUIData = InputHandler.Instance.manager.dicInputUI;
        foreach (var itemData in dicUIData)
        {
            InputActionUIEnum itemKey = itemData.Key;
            InputAction itemValue = itemData.Value;
            itemValue.started += CallBackForInputActionStarted;
        }
    }

    /// <summary>
    /// 注销所有输入事件
    /// </summary>
    public void UnRegisterInputAction()
    {
        Dictionary<InputActionUIEnum, InputAction> dicUIData = InputHandler.Instance.manager.dicInputUI;
        foreach (var itemData in dicUIData)
        {
            InputActionUIEnum itemKey = itemData.Key;
            InputAction itemValue = itemData.Value;
            itemValue.started -= CallBackForInputActionStarted;
        }
    }

    /// <summary>
    /// 回调-输入时间反馈
    /// </summary>
    /// <param name="callback"></param>
    private void CallBackForInputActionStarted(CallbackContext callback)
    {
        if (gameObject.activeSelf)
        {
            this.WaitExecuteEndOfFrame(1, () =>
            {
                OnInputActionForStarted(callback.action.name.GetEnum<InputActionUIEnum>());
            });
        }
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    public virtual void OnClickForButton(Button viewButton)
    {

    }

    /// <summary>
    /// 数据事件点击
    /// </summary>
    /// <param name="inputName"></param>
    public virtual void OnInputActionForStarted(InputActionUIEnum inputType)
    {

    }
}
