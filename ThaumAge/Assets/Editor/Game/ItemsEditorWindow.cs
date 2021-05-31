using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsEditorWindow : EditorWindow
{
    protected string queryItemsIds;
    protected string queryItemsName;
    protected List<ItemsInfoBean> listQueryData = new List<ItemsInfoBean>();
    protected ItemsInfoBean itemsInfoCreate = new ItemsInfoBean();

    protected ItemsInfoService serviceForItemsInfo;
    protected Vector2 scrollPosition;
    [MenuItem("工具/道具生成工具")]
    static void CreateWinds()
    {
        EditorWindow.GetWindow(typeof(ItemsEditorWindow));
    }
    private void OnEnable()
    {
        serviceForItemsInfo = new ItemsInfoService();
    }

    private void OnDisable()
    {
        DestroyImmediate(ItemsHandler.Instance.gameObject);
        DestroyImmediate(BlockHandler.Instance.gameObject);
    }
    public void RefreshData()
    {
        listQueryData.Clear();
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
        UIForItemsList(listQueryData);


        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 创建
    /// </summary>
    protected void UIForCreate()
    {
        UIForItemItems(true, itemsInfoCreate);
    }

    /// <summary>
    /// 方块列表UI
    /// </summary>
    /// <param name="listData"></param>
    protected void UIForItemsList(List<ItemsInfoBean> listData)
    {
        if (CheckUtil.ListIsNull(listData))
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            GUILayout.Space(50);
            ItemsInfoBean itemsInfo = listData[i];
            UIForItemItems(false, itemsInfo);
        }
    }


    /// <summary>
    ///  展示UI
    /// </summary>
    /// <param name="isCreate"></param>
    /// <param name="blockInfo"></param>
    protected void UIForItemItems(bool isCreate, ItemsInfoBean itemsInfo)
    {
        if (itemsInfo == null)
            return;
        GUILayout.BeginHorizontal();
        if (isCreate)
        {
            if (EditorUI.GUIButton("创建物品", 150))
            {
                itemsInfo.link_id = itemsInfo.id;
                itemsInfo.valid = 1;
                bool isSuccess = serviceForItemsInfo.UpdateData(itemsInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("创建失败");
                }
            }
        }
        else
        {
            if (EditorUI.GUIButton("更新物品", 150))
            {
                itemsInfo.link_id = itemsInfo.id;
                bool isSuccess = serviceForItemsInfo.UpdateData(itemsInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("更新失败");
                }
            }
            if (EditorUI.GUIButton("删除物品", 150))
            {
                bool isSuccess = serviceForItemsInfo.DeleteData(itemsInfo.id);
                if (isSuccess)
                {
                    listQueryData.Remove(itemsInfo);
                }
                else
                {
                    LogUtil.LogError("删除失败");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("物品Id", 50);
        itemsInfo.id = EditorUI.GUIEditorText(itemsInfo.id);
        EditorUI.GUIText("名字", 50);
        itemsInfo.name = EditorUI.GUIEditorText(itemsInfo.name);
        EditorUI.GUIText("图标", 50);
        itemsInfo.icon_key = EditorUI.GUIEditorText(itemsInfo.icon_key, 100);
        EditorUI.GUIPic("Assets/Texture/Items/" + itemsInfo.icon_key + ".png");
        itemsInfo.items_type = (int)EditorUI.GUIEnum<ItemsTypeEnum>("物品类型",itemsInfo.items_type,250);

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// <summary>
    /// 查询UI
    /// </summary>
    protected void UIForQuery()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("Id 查询道具", 150))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(queryItemsIds, ',');
            listQueryData = serviceForItemsInfo.QueryDataByIds(ids);
        }
        queryItemsIds = EditorUI.GUIEditorText(queryItemsIds, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("name 查询道具", 150))
        {
            listQueryData = serviceForItemsInfo.QueryDataByName(queryItemsName);
        }
        queryItemsName = EditorUI.GUIEditorText(queryItemsName, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("查询所有道具", 150))
        {
            listQueryData = serviceForItemsInfo.QueryAllData();
        }
        GUILayout.EndHorizontal();
    }
}