using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using System;

[InitializeOnLoad]
public class HierarchySelect
{
    static HierarchySelect()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyShowSelect;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
       
    }

    //选择列表
    public static Dictionary<string, Component> dicSelectObj = new Dictionary<string, Component>();
    public static BaseUIComponent baseUIComponent = null;


    /// <summary>
    /// 视窗改变
    /// </summary>
    private static void OnHierarchyChanged()
    {
        if (!EditorUtil.CheckIsPrefabMode(out var prefabStage))
        {
            return;
        }
        dicSelectObj.Clear();
        baseUIComponent = null;
        GameObject root = prefabStage.prefabContentsRoot;
        baseUIComponent = root.GetComponent<BaseUIComponent>();

        if (baseUIComponent == null) return;
        //设置初始化数据
        Dictionary<string, Type> dicData = ReflexUtil.GetAllNameAndType(baseUIComponent);
        foreach (var itemData in dicData)
        {
            string itemKey = itemData.Key;
            Type itemValue = itemData.Value;
            if (itemKey.Contains("ui_"))
            {
                string componentName = itemKey.Replace("ui_", "");
                if (itemValue != null)
                {
                    Component[] listRootComponent = root.GetComponentsInChildren(itemValue);
                    foreach (Component itemRootComponent in listRootComponent)
                    {
                        if (itemRootComponent.name.Equals(componentName))
                        {
                            dicSelectObj.Add(componentName, itemRootComponent);
                        }
                    }
                }
            }
        }
        return;    
    }

    /// <summary>
    /// 视窗元素
    /// </summary>
    /// <param name="instanceid"></param>
    /// <param name="selectionrect"></param>
    private static void OnHierarchyShowSelect(int instanceid, Rect selectionrect)
    {
        //如果不是编辑模式则不进行操作
        if (!EditorUtil.CheckIsPrefabMode(out var prefabStage))
        {
            return;
        }
        //如果不是UI也不进行操作
        if (baseUIComponent == null)
        {
            return;
        }

        //获取当前obj
        var go = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
        if (go == null)
            return;
        if (baseUIComponent == null)
        {
            baseUIComponent = go.GetComponent<BaseUIComponent>();
        }

        //控制开关
        var selectBox = new Rect(selectionrect);
        selectBox.x = selectBox.xMax;
        selectBox.width = 10;
        //检测是否选中
        bool hasGo = false;
        Component selectComonent = null;
        if (dicSelectObj.TryGetValue(go.name, out selectComonent))
        {
            hasGo = true;
        }
        hasGo = GUI.Toggle(selectBox, hasGo, string.Empty);
        if (hasGo)
        {
            if (!dicSelectObj.ContainsKey(go.name))
            {
                dicSelectObj.Add(go.name, null);
            }
        }
        else
        {
            if (dicSelectObj.ContainsKey(go.name))
            {
                dicSelectObj.Remove(go.name);
            }
        }
        //如果选中了
        if (hasGo)
        {
            //下拉选择
            var selectType = new Rect(selectionrect);
            selectType.x = selectBox.xMax - 170;
            selectType.width = 150;
            //获取该obj下所拥有的所有comnponent
            Component[] componentList = go.GetComponents<Component>();
            string[] listData = new string[componentList.Length];
            int selectComonentIndex = 0;
            //初始化所有可选component;
            for (int i = 0; i < componentList.Length; i++)
            {
                listData[i] = componentList[i].GetType().Name;
                if (selectComonent != null && selectComonent.GetType().Name.Equals(listData[i]))
                {
                    selectComonentIndex = i;
                }
            }
            //设置下拉数据
            selectComonentIndex = EditorGUI.Popup(selectType, selectComonentIndex, listData);
            //如果下拉数据改变
            dicSelectObj[go.name] = componentList[selectComonentIndex];
        }
    }
}