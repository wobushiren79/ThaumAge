using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeEditorWindow : EditorWindow
{
    protected BiomeInfoService serviceForBiomeInfo;

    protected List<BiomeInfoBean> listQueryData = new List<BiomeInfoBean>();
    protected BiomeInfoBean biomeInfoCreate = new BiomeInfoBean();
    protected string queryBiomeName;
    protected string queryBiomeIds;

    protected Vector2 scrollPosition;

    [MenuItem("工具/生态生成工具")]
    static void CreateWinds()
    {
        EditorWindow.GetWindow(typeof(BiomeEditorWindow));
    }

    private void OnEnable()
    {
        serviceForBiomeInfo = new BiomeInfoService();
    }
    private void OnDisable()
    {
        DestroyImmediate(GameDataHandler.Instance.gameObject);
        DestroyImmediate(WorldCreateHandler.Instance.gameObject);
    }

    public void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();

        if (EditorUI.GUIButton("刷新数据"))
        {
            RefreshData();
        }

        GUILayout.Space(50);
        UIForQuery();
        GUILayout.Space(50);
        UIForCreate();
        GUILayout.Space(50);
        UIForBiomeList(listQueryData);
        GUILayout.Space(50);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    public void RefreshData()
    {
        listQueryData.Clear();
    }


    /// <summary>
    /// 查询UI
    /// </summary>
    protected void UIForQuery()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("Id 查询生态", 150))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(queryBiomeIds, ',');
            listQueryData = serviceForBiomeInfo.QueryDataByIds(ids);
        }
        queryBiomeIds = EditorUI.GUIEditorText(queryBiomeIds, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("name 查询生态", 150))
        {
            listQueryData = serviceForBiomeInfo.QueryDataByName(queryBiomeName);
        }
        queryBiomeName = EditorUI.GUIEditorText(queryBiomeName, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("查询所有生态", 150))
        {
            listQueryData = serviceForBiomeInfo.QueryAllData();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 创建
    /// </summary>
    protected void UIForCreate()
    {
        UIForBiomeItem(true, biomeInfoCreate);
    }

    /// <summary>
    /// 生态列表UI
    /// </summary>
    /// <param name="listData"></param>
    protected void UIForBiomeList(List<BiomeInfoBean> listData)
    {
        if (listData.IsNull())
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            GUILayout.Space(50);
            BiomeInfoBean itemBiomeInfo = listData[i];
            UIForBiomeItem(false, itemBiomeInfo);
        }
    }

    /// <summary>
    ///   生态展示UI
    /// </summary>
    /// <param name="isCreate"></param>
    /// <param name="biomeInfo"></param>
    protected void UIForBiomeItem(bool isCreate, BiomeInfoBean biomeInfo)
    {
        if (biomeInfo == null)
            return;
        GUILayout.BeginHorizontal();
        if (isCreate)
        {
            if (EditorUI.GUIButton("创建生态", 150))
            {
                biomeInfo.link_id = biomeInfo.id;
                biomeInfo.valid = 1;
                bool isSuccess = serviceForBiomeInfo.UpdateData(biomeInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("创建失败");
                }
            }
        }
        else
        {
            if (EditorUI.GUIButton("更新生态", 150))
            {
                biomeInfo.link_id = biomeInfo.id;
                bool isSuccess = serviceForBiomeInfo.UpdateData(biomeInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("更新失败");
                }
            }
            if (EditorUI.GUIButton("删除生态", 150))
            {
                bool isSuccess = serviceForBiomeInfo.DeleteData(biomeInfo.id);
                if (isSuccess)
                {
                    listQueryData.Remove(biomeInfo);
                }
                else
                {
                    LogUtil.LogError("删除失败");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("生态Id", 50);
        biomeInfo.id = EditorUI.GUIEditorText(biomeInfo.id);
        EditorUI.GUIText("名字", 50);
        biomeInfo.name = EditorUI.GUIEditorText(biomeInfo.name);
        biomeInfo.id = (int)EditorUI.GUIEnum<BiomeTypeEnum>("生态类型：",(int)biomeInfo.id);
        EditorUI.GUIText("频率", 50);
        biomeInfo.frequency = EditorUI.GUIEditorText(biomeInfo.frequency);
        EditorUI.GUIText("振幅", 50);
        biomeInfo.amplitude = EditorUI.GUIEditorText(biomeInfo.amplitude);
        EditorUI.GUIText("最小高度（默认50）", 150);
        biomeInfo.minHeight = EditorUI.GUIEditorText(biomeInfo.minHeight);
        EditorUI.GUIText("大小", 50);
        biomeInfo.scale = EditorUI.GUIEditorText(biomeInfo.scale);
        GUILayout.EndHorizontal();
    }
}