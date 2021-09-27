﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UGUIUtil
{
    /// <summary>
    /// 获取Icon在指定UIroot下的坐标
    /// </summary>
    /// <param name="tfRoot"></param>
    /// <param name="tfIcon"></param>
    public static Vector3 GetUIRootPosForIcon(Transform tfRoot, Transform tfIcon)
    {
        return tfRoot.InverseTransformPoint(tfIcon.position);
    }


    /// <summary>
    /// 是否点击到了UI
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerUI()
    {
        if (EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0)
        {
            //点击到了UI
            return true;
        }
        else
        {
            //没有点击到UI
            return false;
        }
    }

    /// <summary>
    /// 获得当前点击到的UI物体
    /// </summary>
    public static GameObject GetUICurrentSelect()
    {
        GameObject obj = null;

        GraphicRaycaster[] graphicRaycasters = GameObject.FindObjectsOfType<GraphicRaycaster>();

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();

        foreach (var item in graphicRaycasters)
        {
            item.Raycast(eventData, list);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    obj = list[i].gameObject;
                }
            }
        }

        return obj;
    }

    /// <summary>
    /// 刷新UI大小
    /// </summary>
    /// <param name="rectTransform"></param>
    public static void RefreshUISize(RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}