using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseUIManager : BaseManager
{
    public GameObject objUIContainer;

    public Transform containerUIBase;
    public Transform containerDialog;
    public Transform containerToast;
    public Transform containerPopup;
    public Transform containerOverlay;

    public virtual void Awake()
    {
        GameObject uiContainerModel = LoadResourcesUtil.SyncLoadData<GameObject>("UI/Base/UIContainer");
        objUIContainer = Instantiate(gameObject, uiContainerModel);

        containerUIBase = objUIContainer.FindChild<Transform>("UIBase");
        containerDialog = objUIContainer.FindChild<Transform>("Dialog");
        containerToast = objUIContainer.FindChild<Transform>("Toast");
        containerPopup = objUIContainer.FindChild<Transform>("Popup");
        containerOverlay = objUIContainer.FindChild<Transform>("Overlay");
    }

    /// <summary>
    /// 获取UI容器
    /// </summary>
    /// <param name="uiType"></param>
    public virtual Transform GetUITypeContainer(UITypeEnum uiType)
    {
        switch (uiType)
        {
            case UITypeEnum.UIBase:
                return containerUIBase;
            case UITypeEnum.Dialog:
                return containerDialog;
            case UITypeEnum.Toast:
                return containerToast;
            case UITypeEnum.Popup:
                return containerPopup;
            case UITypeEnum.Overlay:
                return containerOverlay;
        }
        return null;
    }


}
