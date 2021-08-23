using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using System;

[InitializeOnLoad]
[CustomEditor(typeof(BaseUIComponent), true)]
public class InspectorBaseUIComponent : Editor
{
    protected readonly static string scrpitsTemplatesPath = "/Editor/ScrpitsTemplates/UI_BaseUIComponent.txt";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!EditorUtil.CheckIsPrefabMode())
        {
            return;
        }
        GUILayout.Space(50);
        if (EditorUI.GUIButton("生成UICompont脚本", 200))
        {
            HandleForCreateUIComponent();
        }
        if (EditorUI.GUIButton("设置UICompont数据", 200))
        {
            HandleForSetUICompontData();
        }
    }

    ////Hierarchy视图
    //[MenuItem("GameObject/创建/UIComponent", false, 10)]
    ////Projects视图
    //[MenuItem("Assets/创建/UIComponent", false, 10)]
    public static void HandleForCreateUIComponent()
    {
        GameObject objSelect = Selection.activeGameObject;
        string fileName = "UI" + objSelect.name + "Component";
        string baseFileName = "UI" + objSelect.name;

        string templatesPath = Application.dataPath + scrpitsTemplatesPath;   

        if (!EditorUtil.CheckIsPrefabMode(out var prefabStage))
        {
            LogUtil.Log("没有进入编辑模式");
            return;
        }
        string[] path = EditorUtil.GetScriptPath(baseFileName);
        //string path = prefabStage.assetPath;
        //获取最后一个/的索引
        if (path.Length == 0)
        {
            LogUtil.Log("没有名字为"+ baseFileName + "的类,请先创建");
            return;
        }
        //规则替换
        Dictionary<string, string> dicReplace = ReplaceRole("UI" + objSelect.name);
        //创建文件
        EditorUtil.CreateClass(dicReplace, templatesPath, fileName, path[0]);

        EditorUtility.SetDirty(objSelect);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 处理 设置UI的值
    /// </summary>
    public static void HandleForSetUICompontData()
    {
        GameObject objSelect = Selection.activeGameObject;
        if (objSelect == null)
            return;
        BaseUIComponent uiComponent = objSelect.GetComponent<BaseUIComponent>();
        Dictionary<string, object> dicData = ReflexUtil.GetAllNameAndValue(uiComponent);
        foreach (var itemData in dicData)
        {
            string itemKey = itemData.Key;
            object itemValue = itemData.Value;
            if (itemKey.Contains("ui_"))
            {
                //获取选中的控件
                Dictionary<string, Component> dicSelect = HierarchySelect.dicSelectObj;
                //对比选中的控件和属性名字是否一样
                if (dicSelect.TryGetValue(itemKey.Replace("ui_",""),out Component itemComponent))
                {
                    ReflexUtil.SetValueByName(uiComponent, itemKey, itemComponent);
                }
            }
        }
        EditorUtility.SetDirty(objSelect);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 替换规则
    /// </summary>
    /// <param name="scripteContent"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    protected static Dictionary<string, string> ReplaceRole(string className)
    {
        //这里实现自定义的一些规则  
        Dictionary<string, string> dicReplaceData = new Dictionary<string, string>();
        dicReplaceData.Add("#ClassName#", className);
        Dictionary<string, Component> dicSelect = HierarchySelect.dicSelectObj;
        StringBuilder content = new StringBuilder();
        foreach (var itemSelect in dicSelect)
        {
            if (itemSelect.Value == null)
                continue;
            Type type = itemSelect.Value.GetType();
            content.Append("    public " + type.Name + " ui_" + itemSelect.Key + ";\r\n\r\n");
        }
        dicReplaceData.Add("#PropertyList#", content.ToString());
        return dicReplaceData;
    }

}