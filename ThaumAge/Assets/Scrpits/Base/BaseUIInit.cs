﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class BaseUIInit : BaseMonoBehaviour
{
    protected List<string> listEvents = new List<string>();

    public virtual void Awake()
    {
        AutoLinkUI();
        RegisterButtons();
    }

    public virtual void OnDestroy()
    {
        UnRegisterInputAction();
    }

    public virtual void OpenUI()
    {
        RegisterInputAction();
        gameObject.ShowObj(true);
        RefreshUI();
    }

    public virtual void CloseUI()
    {
        UnRegisterInputAction();
        gameObject.ShowObj(false);
        //注销所有事件
        for (int i = 0; i < listEvents.Count; i++)
        {
            string itemEvent = listEvents[i];
            UnRegisterEvent(itemEvent);
        }
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
    public virtual void RegisterInputAction()
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
    public virtual void UnRegisterInputAction()
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
    protected virtual void CallBackForInputActionStarted(CallbackContext callback)
    {
        if (callback.action.name.IsNull())
            return;
        if (gameObject.activeInHierarchy)
        {
            this.WaitExecuteEndOfFrame(1, () =>
            {
                if (gameObject.activeInHierarchy)
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


    public virtual void OnInputActionForStarted(InputActionUIEnum inputType)
    {

    }



    #region 注册事件
    public virtual void RegisterEvent(string eventName, Action action)
    {
        EventHandler.Instance.RegisterEvent(eventName, action);
        listEvents.Add(eventName);
    }

    public virtual void RegisterEvent<A>(string eventName, Action<A> action)
    {
        EventHandler.Instance.RegisterEvent(eventName, action);
        listEvents.Add(eventName);
    }

    public virtual void UnRegisterEvent(string eventName)
    {
        EventHandler.Instance.UnRegisterEvent(eventName);
        listEvents.Remove(eventName);
    }

    public virtual void TriggerEvent(string eventName)
    {
        EventHandler.Instance.TriggerEvent(eventName);
    }
    public virtual void TriggerEvent<A>(string eventName,A data)
    {
        EventHandler.Instance.TriggerEvent(eventName, data);
    }
    #endregion
}