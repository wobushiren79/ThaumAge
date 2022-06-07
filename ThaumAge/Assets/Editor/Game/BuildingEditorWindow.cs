using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingEditorWindow : EditorWindow
{
    protected BuildingInfoService serviceForBuildingInfo;

    protected Vector2 scrollPosition;

    BuildingInfoBean createBuildInfo = new BuildingInfoBean();
    protected BuildingTypeEnum queryBuildingType = BuildingTypeEnum.Mushroom;

    protected List<BuildingInfoBean> listQueryData = new List<BuildingInfoBean>();

    [MenuItem("工具/建筑生成工具")]
    static void CreateWinds()
    {
        EditorWindow.GetWindow(typeof(BuildingEditorWindow));
    }

    private void OnEnable()
    {
        BlockHandler.Instance.manager.InitData();
        serviceForBuildingInfo = new BuildingInfoService();
    }

    private void OnDestroy()
    {
        DestroyImmediate(BlockHandler.Instance.gameObject);
        DestroyImmediate(GameDataHandler.Instance.gameObject);
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
        UIForBuildingItem(createBuildInfo, true);
        GUILayout.Space(50);
        UIForBuildingList(listQueryData);
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
        queryBuildingType = EditorUI.GUIEnum<BuildingTypeEnum>("建筑类型", (int)queryBuildingType);
        if (EditorUI.GUIButton("查询建筑", 150))
        {
            listQueryData = serviceForBuildingInfo.QueryDataById((int)queryBuildingType);
        }
        GUILayout.Space(50);
        if (EditorUI.GUIButton("查询所有建筑", 150))
        {
            listQueryData = serviceForBuildingInfo.QueryAllData();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 列表展示
    /// </summary>
    /// <param name="listData"></param>
    protected void UIForBuildingList(List<BuildingInfoBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            UIForBuildingItem(listData[i], false);
        }
    }

    /// <summary>
    /// 展示
    /// </summary>
    /// <param name="itemData"></param>
    protected void UIForBuildingItem(BuildingInfoBean itemData, bool isCreate)
    {
        GUILayout.BeginHorizontal();

        if (isCreate)
        {
            if (EditorUI.GUIButton("创建建筑", 150))
            {
                GetBuildingData(itemData);
                serviceForBuildingInfo.UpdateData(itemData);
            }
        }
        else
        {
            if (EditorUI.GUIButton("加载建筑", 150))
            {
                SetBuildingData(itemData);
            }
            if (EditorUI.GUIButton("更新建筑", 150))
            {
                GetBuildingData(itemData);
                serviceForBuildingInfo.UpdateData(itemData);
            }
        }

        EditorUI.GUIText("id", 50);
        itemData.id = EditorUI.GUIEditorText(itemData.id, 100);
        EditorUI.GUIText("名称", 50);
        itemData.name_cn = EditorUI.GUIEditorText(itemData.name_cn, 100);
        itemData.name_en = EditorUI.GUIEditorText(itemData.name_en, 100);
        if (isCreate)
        {

        }
        else
        {
            if (EditorUI.GUIButton("删除建筑", 150))
            {
                serviceForBuildingInfo.DeleteData(itemData.id);
            }
        }

        GUILayout.EndHorizontal();
    }

    public void GetBuildingData(BuildingInfoBean itemData)
    {
        GameObject objBuilding = GameObject.Find("Building");
        List<BuildingBean> listBuildingData = new List<BuildingBean>();
        for (int i = 0; i < objBuilding.transform.childCount; i++)
        {
            Transform tfChild = objBuilding.transform.GetChild(i);
            BuildingEditorModel buildingEditor = tfChild.GetComponent<BuildingEditorModel>();
            if (buildingEditor == null)
                continue;
            BuildingBean buildingData = new BuildingBean();
            buildingData.position = Vector3Int.CeilToInt(tfChild.position);
            //buildingData.direction = (int)buildingEditor.direction;
            //buildingData.blockId = (int)buildingEditor.blockType;
            //buildingData.randomRate = buildingEditor.randomRate;
            listBuildingData.Add(buildingData);
        }
        itemData.SetListBuildingData(listBuildingData);
    }

    public void SetBuildingData(BuildingInfoBean itemData)
    {
        GameObject objBuilding = GameObject.Find("Building");
        GameObject objModel = GameObject.Find("Model");
        CptUtil.RemoveChildsByActiveInEditor(objBuilding);
        List<BuildingBean> listData = itemData.listBuildingData;
        if (listData == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            BuildingBean buildingData = listData[i];
            GameObject objItem = Instantiate(objModel);
            objItem.SetActive(true);
            objItem.transform.SetParent(objBuilding.transform);

            BuildingEditorModel buildingEditor = objItem.GetComponent<BuildingEditorModel>();
            //buildingEditor.direction = (DirectionEnum)buildingData.direction;
            //buildingEditor.blockType = (BlockTypeEnum)buildingData.blockId;
            //buildingEditor.randomRate = buildingData.randomRate;
            buildingEditor.transform.position = buildingData.GetPosition();
        }
    }
}