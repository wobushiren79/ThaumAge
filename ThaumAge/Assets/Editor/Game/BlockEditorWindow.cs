using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockEditorWindow : EditorWindow
{
    protected readonly string Path_Block_Png = "Assets/Texture/block.png";
    protected readonly string Path_Block_Textures = "Assets/Texture/Block";

    protected string queryBlockIds;
    protected string queryBlockName;
    protected BlockInfoBean blockInfoCreate = new BlockInfoBean();
    protected List<BlockInfoBean> listQueryData = new List<BlockInfoBean>();
    protected string[] filesNameForTexture = new string[0];

    protected BlockInfoService serviceForBlockInfo;
    protected Vector2 scrollPosition;

    [MenuItem("工具/方块生成工具")]
    static void CreateWinds()
    {
        EditorWindow.GetWindow(typeof(BlockEditorWindow));
    }

    private void OnEnable()
    {
        serviceForBlockInfo = new BlockInfoService();
    }

    private void OnDisable()
    {
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
        UIForCreate();
        GUILayout.Space(50);
        UIForBlockList(listQueryData);
        GUILayout.Space(50);
        UIForCreateTexture();
        GUILayout.Space(50);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    public void RefreshData()
    {
        filesNameForTexture = Directory.GetFiles(Path_Block_Textures);
        listQueryData.Clear();
    }

    /// <summary>
    /// 查询UI
    /// </summary>
    protected void UIForQuery()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("Id 查询方块", 150))
        {
            long[] ids = StringUtil.SplitBySubstringForArrayLong(queryBlockIds, ',');
            listQueryData = serviceForBlockInfo.QueryDataByIds(ids);
        }
        queryBlockIds = EditorUI.GUIEditorText(queryBlockIds, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("name 查询方块", 150))
        {
            listQueryData = serviceForBlockInfo.QueryDataByName(queryBlockName);
        }
        queryBlockName = EditorUI.GUIEditorText(queryBlockName, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("查询所有方块", 150))
        {
            listQueryData = serviceForBlockInfo.QueryAllData();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 创建
    /// </summary>
    protected void UIForCreate()
    {
        UIForBlockItem(true, blockInfoCreate);
    }

    /// <summary>
    /// 方块列表UI
    /// </summary>
    /// <param name="listData"></param>
    protected void UIForBlockList(List<BlockInfoBean> listData)
    {
        if (CheckUtil.ListIsNull(listData))
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            GUILayout.Space(50);
            BlockInfoBean itemBlockInfo = listData[i];
            UIForBlockItem(false, itemBlockInfo);
        }
    }


    /// <summary>
    ///  方块展示UI
    /// </summary>
    /// <param name="isCreate"></param>
    /// <param name="blockInfo"></param>
    protected void UIForBlockItem(bool isCreate, BlockInfoBean blockInfo)
    {
        if (blockInfo == null)
            return;
        GUILayout.BeginHorizontal();
        if (isCreate)
        {
            if (EditorUI.GUIButton("创建方块", 150))
            {
                blockInfo.link_id = blockInfo.id;
                blockInfo.valid = 1;
                bool isSuccess = serviceForBlockInfo.UpdateData(blockInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("创建失败");
                }
            }
        }
        else
        {
            if (EditorUI.GUIButton("更新方块", 150))
            {
                blockInfo.link_id = blockInfo.id;
                bool isSuccess = serviceForBlockInfo.UpdateData(blockInfo);
                if (!isSuccess)
                {
                    LogUtil.LogError("更新失败");
                }
            }
            if (EditorUI.GUIButton("删除方块", 150))
            {
                bool isSuccess = serviceForBlockInfo.DeleteData(blockInfo.id);
                if (isSuccess)
                {
                    listQueryData.Remove(blockInfo);
                }
                else
                {
                    LogUtil.LogError("删除失败");
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorUI.GUIText("方块Id", 50);
        blockInfo.id = EditorUI.GUIEditorText(blockInfo.id);
        EditorUI.GUIText("名字",50);
        blockInfo.name = EditorUI.GUIEditorText(blockInfo.name);
        blockInfo.shape = (int)EditorUI.GUIEnum<BlockShapeEnum>("方块形状：",blockInfo.shape);
        EditorUI.GUIText("重量", 50);
        blockInfo.weight = EditorUI.GUIEditorText(blockInfo.weight);
        EditorUI.GUIText("旋转状态（0不能旋转 1可以旋转）", 200);
        blockInfo.rotate_state = EditorUI.GUIEditorText(blockInfo.rotate_state);
        EditorUI.GUIText("图片", 50);
        blockInfo.uv_position = EditorUI.GUIEditorText(blockInfo.uv_position);
        string[] uvStr = StringUtil.SplitBySubstringForArrayStr(blockInfo.uv_position, '|');
        for (int i = 0; i < uvStr.Length; i++)
        {
            string itemUV = uvStr[i];
            string itemUVPosition = itemUV.Replace(",", "_");
            for (int f = 0; f < filesNameForTexture.Length; f++)
            {
                string fileName = filesNameForTexture[f];
                if (fileName.Contains(".meta"))
                {
                    continue;
                }
                if (fileName.Contains(itemUVPosition))
                {
                    EditorUI.GUIPic(fileName,16,16);
                    break;
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 生成图片UI
    /// </summary>
    protected void UIForCreateTexture()
    {
        if (EditorUI.GUIButton("生成方块图片", 150))
            CreateBlockTexture(2048);
        EditorUI.GUIPic(Path_Block_Png, 2048, 2048);
    }

    /// <summary>
    /// 创建方块图片
    /// </summary>
    /// <param name="size"></param>
    public void CreateBlockTexture(int size)
    {
        string[] filesName = Directory.GetFiles(Path_Block_Textures);
        //生成图片tex
        Texture2D outTexture = new Texture2D(size, size, TextureFormat.RGBA32, true);
        //设置每一个方块所占的区域大小
        int itemSize = size / 128;
        for (int i = 0; i < filesName.Length; i++)
        {
            //获取方块名字
            string fileName = filesName[i];
            if (fileName.Contains(".meta"))
            {
                continue;
            }
            //根据名字获取每个图片所在的位置
            Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>(fileName);
            string[] itemDataArray = StringUtil.SplitBySubstringForArrayStr(itemTex.name, '_');
            int positionStartX = int.Parse(itemDataArray[1]) * itemSize;
            int positionStartY = int.Parse(itemDataArray[0]) * itemSize;

            //设置方块位置
            int width = itemTex.width;
            int height = itemTex.height;
            outTexture.SetPixels(positionStartX, positionStartY, width, height, itemTex.GetPixels());
        }
        //保存图片
        File.WriteAllBytes(Path_Block_Png, outTexture.EncodeToPNG());
        AssetDatabase.Refresh();
    }


}
