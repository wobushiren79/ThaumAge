using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
[CustomEditor(typeof(GameLauncher), true)]
public class GameLauncherUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(50);
        if (EditorUI.GUIButton("复制测试生态数据", 200))
        {
            CopyBiomeTestData();
        }
        if (EditorUI.GUIButton("粘贴生态数据", 200))
        {
            PasteFuctionTestData();
        }
    }

    public void CopyBiomeTestData()
    {
        GameObject objSelect = Selection.activeGameObject;
        GameLauncher gameLauncher = objSelect.GetComponent<GameLauncher>();
        var testData = gameLauncher.testTerrain3DCShaderNoise;
        List<object> listAllData = ReflexUtil.GetAllValue(testData);
        StringBuilder copyData = new StringBuilder();
        for (int i = 0; i < listAllData.Count; i++)
        {
            var itemData = listAllData[i];
            if (i == 0)
            {
                copyData.Append($"{itemData}");
            }
            else
            {
                copyData.Append($"\t{itemData}");
            }
        }
        LogUtil.Log($"复制成功:{copyData.ToString()}");
        GUIUtility.systemCopyBuffer = copyData.ToString();
    }

    public void PasteFuctionTestData()
    {
        string pasteData = GUIUtility.systemCopyBuffer;
        LogUtil.Log($"粘贴内容：{pasteData}");
        string[] pasteDataArray = pasteData.SplitForArrayStr('\t');
        List<string> listName = ReflexUtil.GetAllName<Terrain3DCShaderNoiseLayer>();

        Terrain3DCShaderNoiseLayer testData = new Terrain3DCShaderNoiseLayer();
        // 使用反射设置结构体字段值并影响原始结构体实例
        object boxedStruct = testData; // 将结构体装箱为对象

        var allTypes = ReflexUtil.GetAllNameAndType(boxedStruct);
        for (int i = 0; i < pasteDataArray.Length; i++)
        {
            if (i >= listName.Count)
                break;
            string itemName = listName[i];
            var itemType = allTypes[itemName];
            object itemObj = null;
            if(itemType == typeof(float))
            {
                itemObj = float.Parse(pasteDataArray[i]);     
            }
            else if (itemType == typeof(int))
            {
                itemObj = int.Parse(pasteDataArray[i]);
            }
            else if (itemType == typeof(long))
            {
                itemObj = long.Parse(pasteDataArray[i]);
            }
            else if (itemType == typeof(string))
            {
                itemObj = pasteDataArray[i];
            }
            testData = ReflexUtil.SetValueByNameForStruct(testData, itemName, itemObj);
        }

        GameObject objSelect = Selection.activeGameObject;
        GameLauncher gameLauncher = objSelect.GetComponent<GameLauncher>();
        gameLauncher.testTerrain3DCShaderNoise = testData;
    }
}
