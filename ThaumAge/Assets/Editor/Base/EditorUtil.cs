﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public static class EditorUtil
{

    /// <summary>
    /// 通过资源文件的唯一ID获取选择文件的路径
    /// 注：仅适用于 选中Project 中的物体
    /// </summary>
    /// <returns></returns>
    public static string[] GetSelectionPathByGUIDS()
    {
        string[] strs = Selection.assetGUIDs;
        string[] arrayData = new string[strs.Length];
        for (int i = 0; i < strs.Length; i++)
        {
            string guid = strs[i];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            arrayData[i] = path;
        }
        return arrayData;
    }

    /// <summary>
    /// 获取选中的obj物体获取路径
    /// 注：仅适用于 选中Project 中的物体
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetSelectionPathByObj(GameObject obj)
    {
        if (obj == null)
            return null;
        return AssetDatabase.GetAssetPath(obj);
    }

    /// <summary>
    /// 获取选中obj在场景中的位置
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetSelectionScenePathByObj(GameObject obj)
    {
        if (obj == null)
            return null;
        return AssetDatabase.GetAssetOrScenePath(obj);
    }

    /// <summary>
    /// 获取选中obj物体原文件的路径
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetSelectionSourcePathByObj(GameObject obj)
    {
        GameObject sourceObj = GetSourceGameObject(obj);
        return GetSelectionPathByObj(sourceObj);
    }

    /// <summary>
    /// 获取obj的原obj
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static GameObject GetSourceGameObject(GameObject obj)
    {
        if (obj == null)
            return null;
        return PrefabUtility.GetCorrespondingObjectFromSource(obj);
    }

    /// <summary>
    /// 获取脚本路径
    /// </summary>
    /// <param name="scriptName"></param>
    /// <returns></returns>
    public static string[] GetScriptPath(string scriptName)
    {
        string[] uuids = AssetDatabase.FindAssets(scriptName,new string[] { "Assets" } );
        List<string> listData = new List<string>();
        for (int i = 0; i < uuids.Length; i++)
        {
            string uuid = uuids[i];
            string uuidPath = AssetDatabase.GUIDToAssetPath(uuid);
            if (uuidPath.Contains(scriptName + ".cs"))
            {
                listData.Add(uuidPath.Replace((@"/" + scriptName + ".cs"), ""));
            }
        }
        return listData.ToArray();
    }

    /// <summary>
    /// 创建.cs文件
    /// </summary>
    /// <param name="dicReplace">替换数据</param>
    /// <param name="templatesPath">模板路径</param>
    /// <param name="fileName">文件名（不用加.cs）</param>
    /// <param name="createPath">创建路径</param>
    public static void CreateClass(Dictionary<string,string> dicReplace, string templatesPath, string fileName, string createPath)
    {
        if (templatesPath.IsNull())
        {
            LogUtil.LogError("模板路径为空");
            return;
        }
        if (fileName.IsNull())
        {
            LogUtil.LogError("文件名为空");
            return;
        }
        if (createPath.IsNull())
        {
            LogUtil.LogError("生成路径为空");
            return;
        }
        //读取模板
        string viewScriptContent = File.ReadAllText(templatesPath);
        //替换数据
        foreach (var itemData in dicReplace)
        {
            viewScriptContent = viewScriptContent.Replace(itemData.Key, itemData.Value);
        }
        //创建文件
        FileUtil.CreateTextFile(createPath, fileName + ".cs", viewScriptContent);
    }

    /// <summary>
    /// 检测是否处于Prefab Mode
    /// </summary>
    /// <param name="prefabStage"></param>
    /// <returns></returns>
    public static bool CheckIsPrefabMode(out PrefabStage prefabStage)
    {
        prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            // 当前正处于Prefab Mode
            return true;
        }
        else
        {
            // 当前没有处于Prefab Mode
            return false;
        }
    }
    public static bool CheckIsPrefabMode()
    {
       return  CheckIsPrefabMode(out PrefabStage prefabStage);
    }


}