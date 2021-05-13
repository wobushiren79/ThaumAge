﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogHandler : BaseUIHandler<DialogHandler, DialogManager>
{
    protected override void Awake()
    {
        sortingOrder = 2;
        base.Awake();
        ChangeUIRenderMode(RenderMode.ScreenSpaceCamera);
    }
    public T CreateDialog<T>(DialogEnum dialogType, DialogView.IDialogCallBack callBack, DialogBean dialogBean) where T : DialogView
    {
        return CreateDialog<T>(dialogType, null, null, callBack, dialogBean, 0);
    }
    public T CreateDialog<T>(DialogEnum dialogType, DialogBean dialogBean, Action<DialogView, DialogBean> actionSubmit = null, Action<DialogView, DialogBean> actionCancel = null) where T : DialogView
    {
        return CreateDialog<T>(dialogType, actionSubmit, actionCancel, null, dialogBean, 0);
    }
    public T CreateDialog<T>(DialogEnum dialogType, Action<DialogView, DialogBean> actionSubmit, Action<DialogView, DialogBean> actionCancel, DialogView.IDialogCallBack callBack, DialogBean dialogBean, float delayDelete) where T : DialogView
    {
        string dialogName = EnumUtil.GetEnumName(dialogType);
        GameObject objDialogModel = manager.GetDialogModel(dialogName);
        if (objDialogModel == null)
        {
            LogUtil.LogError("没有找到指定Dialog：" + dialogName);
            return null;
        }

        GameObject objDialog = Instantiate(gameObject, objDialogModel);
        if (objDialog)
        {
            DialogView dialogView = objDialog.GetComponent<DialogView>();
            if (dialogView == null)
                Destroy(objDialog);
            dialogView.SetCallBack(callBack);
            dialogView.SetAction(actionSubmit, actionCancel);
            dialogView.SetData(dialogBean);
            if (delayDelete != 0)
                dialogView.SetDelayDelete(delayDelete);

            //改变焦点
            EventSystem.current.SetSelectedGameObject(objDialog);
            manager.AddDialog(dialogView);
            return dialogView as T;
        }
        else
        {
            LogUtil.LogError("没有实例化Dialog成功：" + dialogName);
            return null;
        }
    }
}
