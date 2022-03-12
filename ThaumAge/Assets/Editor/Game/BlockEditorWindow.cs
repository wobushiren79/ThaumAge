﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BlockEditorWindow : EditorWindow
{
    public static List<BlockMaterialEnum> listBlockMat = new List<BlockMaterialEnum>()
    {
        BlockMaterialEnum.Normal,
        BlockMaterialEnum.BothFace,
        BlockMaterialEnum.BothFaceSwing,
        BlockMaterialEnum.BothFaceSwingUniform,
    };

    protected static readonly string Path_Block_BlockMat = "Assets/Texture/BlockMat";
    protected static readonly string Path_Block_TextureArray = "Assets/Texture/BlockTextureArray";
    protected static readonly string Path_Block_Textures = "Assets/Texture/Block";
    protected static readonly string Path_Block_Mesh = "Assets/Art/FBX/Block";

    protected static readonly string Path_Block_Model = "Assets/Art/FBX/BlockModel";
    protected static readonly string Path_Block_Model_Mat = "Assets/Mats/BlockCustom.mat";
    protected static readonly string Path_Block_Model_Save = "Assets/Prefabs/BlockModel";

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
        UIForBase();
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

    protected void UIForBase()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("刷新数据"))
        {
            RefreshData();
        }
        if (EditorUI.GUIButton("刷新所有方块资源（贴图 mesh数据）", 300))
        {
            CreateBlockTexture(2048, 0);
            CreateBlockMeshData();
        }
        if (EditorUI.GUIButton("生成方块图片", 150))
        {
            CreateBlockTexture(2048, 0);
        }
        if (EditorUI.GUIButton("生成方块mesh数据", 150))
        {
            CreateBlockMeshData();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 查询UI
    /// </summary>
    protected void UIForQuery()
    {
        GUILayout.BeginHorizontal();
        if (EditorUI.GUIButton("Id 查询方块", 150))
        {
            long[] ids = queryBlockIds.SplitForArrayLong(',');
            listQueryData = serviceForBlockInfo.QueryDataByIds(ids);
        }
        queryBlockIds = EditorUI.GUIEditorText(queryBlockIds, 150);
        GUILayout.Space(50);
        if (EditorUI.GUIButton("name 查询方块(中文)", 150))
        {
            listQueryData = serviceForBlockInfo.QueryDataByName(LanguageEnum.cn, queryBlockName);
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
        if (listData.IsNull())
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
        EditorUI.GUIText("名字", 50);
        blockInfo.name_cn = EditorUI.GUIEditorText(blockInfo.name_cn);
        blockInfo.name_en = EditorUI.GUIEditorText(blockInfo.name_en);
        blockInfo.shape = (int)EditorUI.GUIEnum<BlockShapeEnum>("方块形状：", blockInfo.shape);
        EditorUI.GUIText("重量", 50);
        blockInfo.weight = EditorUI.GUIEditorText(blockInfo.weight);
        EditorUI.GUIText("旋转状态（0不能旋转 1可以旋转 2只能正面朝上旋转）", 200);
        EditorUI.GUIText("图片", 50);
        blockInfo.uv_position = EditorUI.GUIEditorText(blockInfo.uv_position);
        string[] uvStr = blockInfo.uv_position.SplitForArrayStr('|');
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
                    EditorUI.GUIPic(fileName, 16, 16);
                    break;
                }
            }
        }
        EditorUI.GUIText("模型名称", 50);
        blockInfo.model_name = EditorUI.GUIEditorText(blockInfo.model_name, 100);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 生成图片UI
    /// </summary>
    protected void UIForCreateTexture()
    {
        string nameBothFaceSwingUniform = BlockMaterialEnum.BothFaceSwingUniform.GetEnumName();
        string nameBothFaceSwing = BlockMaterialEnum.BothFaceSwing.GetEnumName();
        string nameBothFace = BlockMaterialEnum.BothFace.GetEnumName();

        EditorUI.GUIPic($"{Path_Block_BlockMat}/BlockNomral_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_Block_BlockMat}/Block{nameBothFace}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_Block_BlockMat}/Block{nameBothFaceSwing}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_Block_BlockMat}/Block{nameBothFaceSwingUniform}_0.png", 2048, 2048);
    }


    /// <summary>
    /// 创建方块动画贴图列表
    /// </summary>
    /// <param name="size"></param>
    /// <param name="frameNumber"></param>
    public static void CreateBlockTextureArrayTexture(int size, int frameNumber)
    {
        for (int i = 0; i < frameNumber; i++)
        {
            CreateBlockTexture(size, i);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块TextureArray
    /// </summary>
    /// <param name="size"></param>
    /// <param name="frameNumber"></param>
    public static void CreateBlockTextureArray(int size, int frameNumber)
    {
        for (int m = 0; m < listBlockMat.Count; m++)
        {
            string nameOfMat = listBlockMat[m].GetEnumName();

            Texture2DArray arrayNormal = null;

            for (int i = 0; i < frameNumber; i++)
            {
                //根据名字获取每个图片所在的位置
                Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>($"{Path_Block_BlockMat}/Block{nameOfMat}_{i}.png");

                if (arrayNormal == null)
                {
                    arrayNormal = new Texture2DArray(size, size, frameNumber, itemTex.graphicsFormat, TextureCreationFlags.None);
                    arrayNormal.filterMode = FilterMode.Point;
                    arrayNormal.wrapMode = TextureWrapMode.Repeat;
                }

                Graphics.CopyTexture(itemTex, 0, arrayNormal, i);
                //arrayNormal.SetPixels32(itemTex.GetPixels32(), i);
            }
            string pathSave = $"{Path_Block_TextureArray}/BlockTextureArrary_{nameOfMat}.asset";
            EditorUtil.CreateAsset(arrayNormal, pathSave);

            AssetDatabase.ImportAsset(pathSave);
        }

        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块图片
    /// </summary>
    /// <param name="size"></param>
    public static void CreateBlockTexture(int size, int frameIndex)
    {
        for (int m = 0; m < listBlockMat.Count; m++)
        {
            //生成图片tex
            Texture2D outTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            outTexture.filterMode = FilterMode.Point;
            outTexture.SetPixels(new Color[size * size]);

            //设置每一个方块所占的区域大小
            int itemSize = size / 128;

            string nameOfMat = listBlockMat[m].GetEnumName();
            string[] filesName = Directory.GetFiles($"{Path_Block_Textures}/{nameOfMat}");

            for (int i = 0; i < filesName.Length; i++)
            {
                //获取方块名字
                string fileName = filesName[i];

                if (fileName.Contains(".meta"))
                    continue;
                //根据名字获取每个图片所在的位置
                Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>(fileName);
                if (itemTex == null)
                    continue;
                string[] itemDataArray = itemTex.name.SplitForArrayStr('_');

                //设置方块位置
                int width = itemTex.width;
                int height = itemTex.height;
                int sizeWH = width > height ? height : width;
                //如果是有动画的贴图 则按照帧序号 设置对应的忒图

                Texture2D useTex;
                if (width > height || height > width)
                {
                    int numberTex = width > height ? width / height : height / width;
                    int startIndex = frameIndex % numberTex;
                    useTex = new Texture2D(sizeWH, sizeWH);
                    Color[] pixelTemp = itemTex.GetPixels(startIndex * sizeWH, 0, sizeWH, sizeWH);
                    useTex.SetPixels(pixelTemp);
                }
                else
                {
                    useTex = itemTex;
                }
                int positionStartX = int.Parse(itemDataArray[2]) * itemSize;
                int positionStartY = int.Parse(itemDataArray[1]) * itemSize;
                outTexture.SetPixels(positionStartX, positionStartY, sizeWH, sizeWH, useTex.GetPixels());
            }
            //保存图片
            string pathBlock = $"{Path_Block_BlockMat}/Block{nameOfMat}_{frameIndex}.png";
            File.WriteAllBytes(pathBlock, outTexture.EncodeToPNG());

            EditorUtil.RefreshAsset();

            var itemImporter = AssetImporter.GetAtPath(pathBlock) as TextureImporter;
            itemImporter.textureType = TextureImporterType.Default;
            itemImporter.isReadable = true;
            itemImporter.textureCompression = TextureImporterCompression.CompressedHQ;
            itemImporter.mipmapEnabled = false;
            itemImporter.filterMode = FilterMode.Point;

            var settingPC = itemImporter.GetPlatformTextureSettings("Standalone");
            settingPC.format = TextureImporterFormat.DXT5;
            itemImporter.SetPlatformTextureSettings(settingPC);
            AssetDatabase.ImportAsset(pathBlock);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块贴图
    /// </summary>
    public static void CreateBlockTexture(int blockTextureSize, List<BlockModelCreateBean> listCreateData)
    {
        //生成图片tex
        Texture2D outTexture = new Texture2D(blockTextureSize, blockTextureSize, TextureFormat.RGBA32, false);
        outTexture.filterMode = FilterMode.Point;
        outTexture.SetPixels(new Color[blockTextureSize * blockTextureSize]);

        for (int i = 0; i < listCreateData.Count; i++)
        {
            BlockModelCreateBean itemCreateData = listCreateData[i];
            outTexture.SetPixels(itemCreateData.startUV.x, itemCreateData.startUV.y, itemCreateData.texureBlock.width, itemCreateData.texureBlock.width, itemCreateData.texureBlock.GetPixels());
        }

        //保存图片
        string pathBlock = $"{Path_Block_Textures}/BlockCommon.png";
        File.WriteAllBytes(pathBlock, outTexture.EncodeToPNG());

        EditorUtil.RefreshAsset();

        var itemImporter = AssetImporter.GetAtPath(pathBlock) as TextureImporter;
        itemImporter.textureType = TextureImporterType.Default;
        itemImporter.isReadable = true;
        itemImporter.textureCompression = TextureImporterCompression.CompressedHQ;
        itemImporter.mipmapEnabled = false;
        itemImporter.filterMode = FilterMode.Point;

        var settingPC = itemImporter.GetPlatformTextureSettings("Standalone");
        settingPC.format = TextureImporterFormat.DXT5Crunched;
        itemImporter.SetPlatformTextureSettings(settingPC);
        AssetDatabase.ImportAsset(pathBlock);
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建所有方块的mesh数据
    /// </summary>
    public static void CreateBlockMeshData()
    {
        FileInfo[] files = FileUtil.GetFilesByPath($"{Path_Block_Mesh}");
        if (files.IsNull())
        {
            LogUtil.Log("CreateBlockMeshData Fail No Block");
            return;
        }
        AddressableAssetGroup addressableAssetGroup = AddressableUtil.FindOrCreateGroup("BlockMesh");
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo itemFile = files[i];
            if (itemFile.Name.Contains(".meta"))
                continue;
            if (itemFile.Name.Contains("_Model"))
                continue;
            LogUtil.Log($"CreateBlockMeshData:{itemFile.Name}");
            GameObject obj = EditorUtil.GetAssetByPath<GameObject>($"{Path_Block_Mesh}/{itemFile.Name}");
            MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
            Collider collider = obj.GetComponentInChildren<Collider>();

            MeshData meshData;
            if (meshFilter != null)
            {
                meshData = new MeshData(collider, meshFilter.sharedMesh, 0.625f, new Vector3(0.5f, 0f, 0.5f));
            }
            else
            {
                meshData = new MeshData(collider, 0.625f, new Vector3(0.5f, 0f, 0.5f));
            }
            string jsonData = JsonUtil.ToJson(meshData);
            string saveFileName = $"{itemFile.Name.Replace(".prefab", "").Replace(".obj", "")}";
            //创建文件
            FileUtil.CreateTextFile($"{Application.dataPath}/Prefabs/BlockMesh", $"{saveFileName}.txt", jsonData);
            //添加到addressable中
            string addressName = $"Assets/Prefabs/BlockMesh/{saveFileName}.txt";
            AddressableUtil.AddAssetEntry(addressableAssetGroup, addressName, addressName);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块模型
    /// </summary>
    public static void CreateBlockModel(int blockTextureSize)
    {
        try
        {
            FileInfo[] files = FileUtil.GetFilesByPath($"{Path_Block_Model}");
            if (files.IsNull())
            {
                LogUtil.Log("CreateBlockModel Fail No Block");
                return;
            }
            FileUtil.DeleteAllFile($"{Path_Block_Model_Save}");
            //所有的创建数据
            List<BlockModelCreateBean> listCreateData = new List<BlockModelCreateBean>();
            //所有像素的最后一个UV坐标位置
            Dictionary<int, Vector2Int> dicUVPosition = new Dictionary<int, Vector2Int>();
            //当前到第几排像素
            int currentPixel = 0;
            for (int i = 0; i < files.Length; i++)
            {
                EditorUI.GUIShowProgressBar("资源刷新", $"刷新方块模型中（{i}/{files.Length}）", (float)i / files.Length);
                FileInfo itemFile = files[i];
                if (itemFile.Name.Contains(".meta"))
                    continue;
                if (itemFile.Name.Contains(".dae"))
                {
                    //获取老方块
                    GameObject obj = EditorUtil.GetAssetByPath<GameObject>($"{Path_Block_Model}/{itemFile.Name}");
                    MeshFilter objMeshFilter = obj.GetComponentInChildren<MeshFilter>();

                    //生成新的方块
                    string nameNew = $"{itemFile.Name.Replace(".dae", "")}";
                    GameObject objNew = new GameObject(nameNew);
                    MeshRenderer objNewMeshRenderer = objNew.AddComponent<MeshRenderer>();
                    MeshFilter objNewMeshFilter = objNew.AddComponent<MeshFilter>();
                    //设置mesh
                    objNewMeshFilter.sharedMesh = objMeshFilter.sharedMesh;
                    //设置材质
                    objNewMeshRenderer.sharedMaterial = EditorUtil.GetAssetByPath<Material>(Path_Block_Model_Mat);

                    EditorUtil.CreatePrefab(objNew, $"{Path_Block_Model_Save}/{nameNew}.prefab");
                    EditorUtil.RefreshAsset(objNew);
                    DestroyImmediate(objNew);

                    //获取对应的材质贴图
                    string texPath = $"{Path_Block_Model}/{nameNew}_texture0.png";
                    Texture2D texItem = EditorUtil.GetAssetByPath<Texture2D>(texPath);
                    if (texItem == null)
                    {
                        texPath = $"{Path_Block_Model}/{nameNew}.png";
                        texItem = EditorUtil.GetAssetByPath<Texture2D>(texPath);
                    }

                    //如果有贴图 则开始生成数据
                    if (texItem != null)
                    {
                        //首先设置图片的一些属性
                        EditorUtil.SetTextureData(texPath);


                        int sizeTexture = texItem.width;
                        BlockModelCreateBean blockModelCreateData = new BlockModelCreateBean();
                        blockModelCreateData.nameBlock = nameNew;
                        blockModelCreateData.uvScaleSize = blockTextureSize / sizeTexture;
                        blockModelCreateData.texureBlock = texItem;
                        //获取该像素到达的位置
                        if (dicUVPosition.TryGetValue(sizeTexture, out Vector2Int uvPosition))
                        {
                            //首先检测是否到达最后一个
                            if (blockTextureSize - uvPosition.x >= sizeTexture)
                            {
                                blockModelCreateData.startUV = new Vector2Int(uvPosition.x + sizeTexture, uvPosition.y);
                                dicUVPosition[sizeTexture] = new Vector2Int(uvPosition.x + sizeTexture, uvPosition.y);
                            }
                            else
                            {
                                //如果已经是最后一个了 则下一排
                                currentPixel += sizeTexture;
                                dicUVPosition[sizeTexture] = new Vector2Int(0, currentPixel);
                                blockModelCreateData.startUV = new Vector2Int(0, currentPixel);
                            }
                        }
                        else
                        {
                            //如果是第一次添加
                            dicUVPosition.Add(texItem.width, new Vector2Int(0, currentPixel));
                            blockModelCreateData.startUV = new Vector2Int(0, currentPixel);
                            currentPixel += sizeTexture;
                        }
                        listCreateData.Add(blockModelCreateData);
                    }
                }
            }
            //创建方块贴图
            CreateBlockTexture(blockTextureSize, listCreateData);
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

}
