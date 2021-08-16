﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelEditorWindow : EditorWindow
{
    [MenuItem("Custom/Excel")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(ExcelEditorWindow));
    }

    public string excelFolderPath = "";
    public string entityFolderPath = "";
    public string jsonFolderPath = "";

    private void OnEnable()
    {
        excelFolderPath = Application.dataPath + "/Data/Excel";
        entityFolderPath = Application.dataPath + "/Scrpits/Bean/MVC/Game";
        //jsonFolderPath = Application.streamingAssetsPath + "/JsonText";
        jsonFolderPath = Application.dataPath + "/Resources/JsonText";
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("选择Excel所在文件夹", 200))
        {
            excelFolderPath = EditorUI.GetFolderPanel("选择目录");
        }
        if (EditorUI.GUIButton("打开所在文件夹", 200))
        {
            EditorUI.OpenFolder(excelFolderPath);
        }
        GUILayout.EndHorizontal();
        excelFolderPath = EditorUI.GUIEditorText(excelFolderPath, 500);

        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("选择Entity所在文件夹", 200))
        {
            entityFolderPath = EditorUI.GetFolderPanel("选择目录");
        }
        if (EditorUI.GUIButton("打开所在文件夹", 200))
        {
            EditorUI.OpenFolder(entityFolderPath);
        }
        GUILayout.EndHorizontal();
        entityFolderPath = EditorUI.GUIEditorText(entityFolderPath, 500);

        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("选择Json文本所在文件夹", 200))
        {
            jsonFolderPath = EditorUI.GetFolderPanel("选择目录");
        }
        if (EditorUI.GUIButton("打开所在文件夹", 200))
        {
            EditorUI.OpenFolder(jsonFolderPath);
        }
        GUILayout.EndHorizontal();
        jsonFolderPath = EditorUI.GUIEditorText(jsonFolderPath, 500);

        GUILayout.Space(50);

        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("生成相关Entity", 200))
        {
            CreateEntities();
        }
        if (EditorUI.GUIButton("Excel转Json文本", 200))
        {
            ExcelToJson();
        }
        GUILayout.EndHorizontal();
    }

    protected void ExcelToJson()
    {
        if (CheckUtil.StringIsNull(excelFolderPath))
        {
            LogUtil.LogError("Excel文件目录为null");
            return;
        }
        if (CheckUtil.StringIsNull(jsonFolderPath))
        {
            LogUtil.LogError("Json文件目录为null");
            return;
        }
        FileInfo[] fileInfos = FileUtil.GetFilesByPath(excelFolderPath);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo fileInfo = fileInfos[i];
            if (fileInfo.Name.Contains(".meta"))
                continue;
            string filePath = fileInfo.FullName;
            FileStream fs;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                LogUtil.LogError("请先关闭对应的Excel文档");
                return;
            }
            try
            {
                ExcelPackage ep = new ExcelPackage(fs);
                //获得所有工作表
                ExcelWorksheets workSheets = ep.Workbook.Worksheets;
                List<object> lst = new List<object>();
                //遍历所有工作表
                for (int w = 1; w <= workSheets.Count; w++)
                {
                    //当前工作表 
                    ExcelWorksheet sheet = workSheets[w];
                    //初始化集合
                    lst.Clear();
                    //横排
                    int columnCount = sheet.Dimension.End.Column;
                    //竖排
                    int rowCount = sheet.Dimension.End.Row;

                    Assembly ab = Assembly.Load("Assembly-CSharp");
                    Type type = ab.GetType(sheet.Name + "Bean");

                    //从第四行开始，前3行分别是属性名字，属性字段，属性描述
                    for (int row = 4; row <= rowCount; row++)
                    {

                        if (type == null)
                        {
                            LogUtil.LogError("你还没有创建对应的实体类!");
                            return;
                        }
                        if (!Directory.Exists(jsonFolderPath))
                            Directory.CreateDirectory(jsonFolderPath);
                        object o = ab.CreateInstance(type.ToString());
                        for (int column = 1; column <= columnCount; column++)
                        {
                            FieldInfo fieldInfo = type.GetField(sheet.Cells[w, column].Text); //先获得字段信息，方便获得字段类型
                            System.Object value = Convert.ChangeType(sheet.Cells[row, column].Text, fieldInfo.FieldType);
                            type.GetField(sheet.Cells[1, column].Text).SetValue(o, value);
                        }
                        lst.Add(o);
                    }
                    //写入json文件
                    string jsonPath = $"{jsonFolderPath}/{sheet.Name}.txt";
                    if (!File.Exists(jsonPath))
                    {
                        File.Create(jsonPath).Dispose();
                    }
                    string jsonData = JsonUtil.ToJsonByNet(lst);
                    File.WriteAllText(jsonPath, jsonData);
                }
                LogUtil.Log("转换完成");
            }
            catch (Exception e)
            {
                LogUtil.LogError(e.ToString());
            }
            finally
            {
                fs.Close();
            }

        }
    }

    void CreateEntities()
    {
        if (CheckUtil.StringIsNull(excelFolderPath))
        {
            LogUtil.LogError("Excel文件目录为null");
            return;
        }
        if (CheckUtil.StringIsNull(entityFolderPath))
        {
            LogUtil.LogError("Entity文件目录为null");
            return;
        }
        FileInfo[] fileInfos = FileUtil.GetFilesByPath(excelFolderPath);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo fileInfo = fileInfos[i];
            if (fileInfo.Name.Contains(".meta"))
                continue;
            string filePath = fileInfo.FullName;
            FileStream fs;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                LogUtil.LogError("请先关闭对应的Excel文档");
                return;
            }
            try
            {
                ExcelPackage ep = new ExcelPackage(fs);

                //获得所有工作表
                ExcelWorksheets workSheets = ep.Workbook.Worksheets;
                //遍历所有工作表
                for (int w = 1; w <= workSheets.Count; w++)
                {
                    CreateEntity(workSheets[w]);
                }
                AssetDatabase.Refresh();
                LogUtil.Log("生成完成");
            }
            catch
            {

            }
            finally
            {
                fs.Close();
            }
        }
    }

    void CreateEntity(ExcelWorksheet sheet)
    {
        string dir = entityFolderPath;
        string path = $"{dir}/{sheet.Name}Bean.cs";
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"using System;");
        sb.AppendLine($"\t[Serializable]");
        sb.AppendLine($"\tpublic class {sheet.Name}Bean");
        sb.AppendLine("\t{");
        //遍历sheet首行每个字段描述的值
        for (int i = 1; i <= sheet.Dimension.End.Column; i++)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendLine($"\t\t///{sheet.Cells[3, i].Text}");
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine($"\t\tpublic {sheet.Cells[2, i].Text} {sheet.Cells[1, i].Text};");
        }
        sb.AppendLine("\t}");
        try
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Dispose(); //避免资源占用
            }
            File.WriteAllText(path, sb.ToString());
        }
        catch (System.Exception e)
        {
            LogUtil.LogError($"Excel转json时创建对应的实体类出错，实体类为：{sheet.Name},e:{e.Message}");
        }
    }
}