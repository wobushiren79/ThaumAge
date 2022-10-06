using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BlockEditorWindow : EditorWindow
{

    public static readonly string Path_BlockTexturesMat = "Assets/Texture/BlockTexturesMat";
    public static readonly string Path_BlockTextureArray = "Assets/Texture/BlockTextureArray";
    public static readonly string Path_Block_Textures = "Assets/Texture/Block";

    public static readonly string Path_Block_MeshModel = "Assets/Prefabs/BlockMeshModel";
    public static readonly string Path_Prefabs_BlockMat = "Assets/Prefabs/Mats";

    public static readonly string Path_FBX_BlockModelCommon = "Assets/Art/FBX/BlockModelCommon";
    public static readonly string Path_FBX_BlockModelCustom = "Assets/Art/FBX/BlockModelCustom";

    public static readonly string Path_BlockMatCommon = "Assets/Mats/BlockCommon.mat";
    public static readonly string Path_BlockMatCustom = "Assets/Prefabs/Mats/BlockCustom_0.mat";

    public static readonly string Path_Block_Model_Save = "Assets/Prefabs/BlockModel";

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
        string nameNormal = BlockMaterialEnum.Normal.GetEnumName();
        string nameTransparent = BlockMaterialEnum.Transparent.GetEnumName();

        EditorUI.GUIPic($"{Path_BlockTexturesMat}/Block{nameNormal}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_BlockTexturesMat}/Block{nameBothFace}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_BlockTexturesMat}/Block{nameBothFaceSwing}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_BlockTexturesMat}/Block{nameBothFaceSwingUniform}_0.png", 2048, 2048);
        EditorUI.GUIPic($"{Path_BlockTexturesMat}/Block{nameTransparent}_0.png", 2048, 2048);
    }

    /// <summary>
    /// 创建方块TextureArray
    /// </summary>
    /// <param name="size"></param>
    /// <param name="frameNumber"></param>
    public static void CreateBlockTextureArray(int size, int frameNumber, List<string> listBlockName)
    {
        for (int m = 0; m < listBlockName.Count; m++)
        {
            string nameOfMat = listBlockName[m];

            Texture2DArray arrayNormal = null;

            for (int i = 0; i < frameNumber; i++)
            {
                //根据名字获取每个图片所在的位置
                Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>($"{Path_BlockTexturesMat}/{nameOfMat}_{i}.png");

                if (arrayNormal == null)
                {
                    arrayNormal = new Texture2DArray(size, size, frameNumber, itemTex.graphicsFormat, TextureCreationFlags.None);
                    arrayNormal.filterMode = FilterMode.Point;
                    arrayNormal.wrapMode = TextureWrapMode.Repeat;
                }

                Graphics.CopyTexture(itemTex, 0, arrayNormal, i);
                //arrayNormal.SetPixels32(itemTex.GetPixels32(), i);
            }
            string pathSave = $"{Path_BlockTextureArray}/BlockTextureArrary_{nameOfMat}.asset";
            EditorUtil.CreateAsset(arrayNormal, pathSave);

            AssetDatabase.ImportAsset(pathSave);
        }

        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块动画贴图列表
    /// </summary>
    /// <param name="size"></param>
    /// <param name="frameNumber"></param>
    public static void CreateBlockAnimTexture(int size, int frameNumber, string resPath, List<string> listBlockArrayName)
    {
        for (int i = 0; i < frameNumber; i++)
        {
            CreateBlockTexture(size, i, resPath, listBlockArrayName);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块图片
    /// </summary>
    /// <param name="size"></param>
    public static void CreateBlockTexture(int size, int frameIndex, string pathRes, List<string> listBlockMatName)
    {
        for (int m = 0; m < listBlockMatName.Count; m++)
        {
            //生成图片tex
            Texture2D outTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            outTexture.filterMode = FilterMode.Point;
            outTexture.SetPixels(new Color[size * size]);

            //设置每一个方块所占的区域大小
            int itemSize = size / 128;

            string nameOfMat = listBlockMatName[m];
            string[] filesName = Directory.GetFiles($"{pathRes}/{nameOfMat}");

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
            string pathBlock = $"{Path_BlockTexturesMat}/{nameOfMat}_{frameIndex}.png";
            File.WriteAllBytes(pathBlock, outTexture.EncodeToPNG());

            EditorUtil.RefreshAsset();

            var itemImporter = AssetImporter.GetAtPath(pathBlock) as TextureImporter;
            itemImporter.textureType = TextureImporterType.Default;
            itemImporter.isReadable = true;
            itemImporter.textureCompression = TextureImporterCompression.Uncompressed;
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
    /// 创建方块贴图列表
    /// </summary>
    public static void CreateBlockAnimTexture(int blockTextureSize, int frameNumber, string savePath, string saveName, List<BlockModelCreateBean> listCreateData)
    {
        for (int i = 0; i < frameNumber; i++)
        {
            CreateBlockTexture(blockTextureSize, savePath, saveName, i, listCreateData);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块贴图
    /// </summary>
    public static void CreateBlockTexture(int blockTextureSize, string savePath, string saveName, int frameIndex, List<BlockModelCreateBean> listCreateData)
    {
        //生成图片tex
        Texture2D outTexture = new Texture2D(blockTextureSize, blockTextureSize, TextureFormat.RGBA32, false);
        outTexture.SetPixels(new Color[blockTextureSize * blockTextureSize]);

        for (int i = 0; i < listCreateData.Count; i++)
        {
            BlockModelCreateBean itemCreateData = listCreateData[i];
            //Graphics.CopyTexture(itemCreateData.texureBlock, 0,0,0,0, itemCreateData.texureBlock.width, itemCreateData.texureBlock.width, outTexture,0, 0, itemCreateData.startPixel.x, itemCreateData.startPixel.y);
            //获取当前帧的贴图
            int numberTex = itemCreateData.listTexureBlock.Count;
            int startIndex = frameIndex % numberTex;
            Color32[] colorPixel = itemCreateData.listTexureBlock[startIndex].GetPixels32();
            outTexture.SetPixels32(itemCreateData.startPixel.x, itemCreateData.startPixel.y, itemCreateData.texureSize, itemCreateData.texureSize, colorPixel);
        }

        //保存图片
        string saveCompletePath = $"{savePath}/{saveName}_{frameIndex}.png";
        File.WriteAllBytes(saveCompletePath, outTexture.EncodeToPNG());

        EditorUtil.RefreshAsset();
        EditorUtil.SetTextureData(saveCompletePath, format: TextureImporterFormat.DXT5, textureImporterCompression: TextureImporterCompression.Uncompressed);
    }

    /// <summary>
    /// 创建所有方块的mesh数据
    /// </summary>
    public static void CreateBlockMeshData()
    {
        EditorUtil.RefreshAsset();
        FileInfo[] files = FileUtil.GetFilesByPath($"{Path_Block_MeshModel}");
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
            LogUtil.Log($"CreateBlockMeshData:{itemFile.Name}");
            GameObject obj = EditorUtil.GetAssetByPath<GameObject>($"{Path_Block_MeshModel}/{itemFile.Name}");
            Collider colliderModel = obj.GetComponentInChildren<Collider>();
            //首先获取主体
            Transform tfModel = obj.transform.Find("Model");
            MeshFilter meshFilterModel = null;
            if (tfModel != null)
            {
                meshFilterModel = tfModel.GetComponentInChildren<MeshFilter>();
            }
            MeshDataCustom meshData = null;
            if (meshFilterModel != null)
            {
                Vector3 offsetPosition = new Vector3(0.5f, 0f, 0.5f);
                if (tfModel != null)
                {
                    offsetPosition += (tfModel.localPosition - new Vector3(0.5f, 0.5f, 0.5f));
                    offsetPosition += meshFilterModel.transform.localPosition + new Vector3(0f, 0.5f, 0f);
                }
                if (meshFilterModel.sharedMesh != null)
                {
                    meshData = new MeshDataCustom(colliderModel, meshFilterModel.sharedMesh, 0.03125f, offsetPosition, meshFilterModel.transform.localEulerAngles);
                }
                else
                {
                    Debug.LogError("生成数据失败 meshFilterModel:" + meshFilterModel.name);
                }
            }
            else
            {
                meshData = new MeshDataCustom(colliderModel, 0.03125f, new Vector3(0.5f, 0f, 0.5f), Vector3.zero);
            }
            //获取Other
            List<Mesh> listMesh0ther = new List<Mesh>(); ;
            List<float> listSizeOther = new List<float>();
            List<Vector3> listOffsetOther = new List<Vector3>();
            List<Vector3> listRotateOther = new List<Vector3>();
            for (int f = 1; f < 10; f++)
            {
                Transform tfOther = obj.transform.Find($"Other{f}");
                if (tfOther == null)
                    continue;
                MeshFilter meshFiltertfOther = tfOther.GetComponentInChildren<MeshFilter>();
                Collider colliderOther = tfOther.GetComponentInChildren<Collider>();

                if (meshFiltertfOther != null)
                {
                    Vector3 offsetPosition = new Vector3(0.5f, 0f, 0.5f);
                    offsetPosition += (tfOther.localPosition - new Vector3(0.5f, 0.5f, 0.5f));
                    offsetPosition += meshFiltertfOther.transform.localPosition + new Vector3(0f, 0.5f, 0f);
                    listSizeOther.Add(0.03125f);
                    listOffsetOther.Add(offsetPosition);
                    listMesh0ther.Add(meshFiltertfOther.sharedMesh);
                    listRotateOther.Add(meshFiltertfOther.transform.localEulerAngles);
                }
            }
            if (!listMesh0ther.IsNull())
            {
                meshData.SetOtherMeshData(listMesh0ther, listSizeOther, listOffsetOther, listRotateOther);
            }

            string jsonData = JsonUtil.ToJson(meshData);
            string saveFileName = $"{itemFile.Name.Replace(".prefab", "").Replace(".obj", "")}";
            //创建文件
            FileUtil.CreateTextFile($"{Application.dataPath}/Prefabs/BlockMeshData", $"{saveFileName}.txt", jsonData);
            //添加到addressable中
            string addressName = $"Assets/Prefabs/BlockMeshData/{saveFileName}.txt";
            EditorUtil.RefreshAsset();
            AddressableUtil.AddAssetEntry(addressableAssetGroup, addressName, addressName);
        }
        EditorUtil.RefreshAsset();
    }

    /// <summary>
    /// 创建方块模型
    /// </summary>
    public static void CreateBlockModel(int blockTextureSize, string pathRes, string pathSaveTexure, string saveName, string pathMatBlock, int textureArrayNumber = 1)
    {
        try
        {
            FileInfo[] files = FileUtil.GetFilesByPath($"{pathRes}");
            if (files.IsNull())
            {
                LogUtil.Log("CreateBlockModel Fail No Block");
                return;
            }
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
                    //生成新的方块
                    string nameNew = $"{itemFile.Name.Replace(".dae", "")}";

                    //获取对应的材质贴图
                    string texPath = $"{pathRes}/{nameNew}_texture";
                    List<Texture2D> listTexItem = new List<Texture2D>();
                    for (int t = 0; t < textureArrayNumber; t++)
                    {
                        Texture2D texItem = EditorUtil.GetAssetByPath<Texture2D>($"{texPath}{t}.png");
                        if (texItem != null)
                            listTexItem.Add(texItem);
                    }

                    //如果有贴图 则开始生成数据
                    if (!listTexItem.IsNull())
                    {
                        //首先设置图片的一些属性
                        for (int t = 0; t < listTexItem.Count; t++)
                        {
                            EditorUtil.SetTextureData($"{texPath}{t}.png");
                        }

                        int sizeTexture = listTexItem[0].width > listTexItem[0].height ? listTexItem[0].height : listTexItem[0].width;
                        BlockModelCreateBean blockModelCreateData = new BlockModelCreateBean();
                        blockModelCreateData.nameBlock = nameNew;
                        blockModelCreateData.uvScaleSize = blockTextureSize / sizeTexture;
                        blockModelCreateData.listTexureBlock = listTexItem;
                        blockModelCreateData.texureSize = sizeTexture;
                        //获取该像素到达的位置
                        if (dicUVPosition.TryGetValue(sizeTexture, out Vector2Int uvPosition))
                        {
                            //首先检测是否到达最后一个
                            if (blockTextureSize - uvPosition.x >= sizeTexture)
                            {
                                dicUVPosition[sizeTexture] = new Vector2Int(uvPosition.x + sizeTexture, uvPosition.y);
                                blockModelCreateData.startPixel = new Vector2Int(uvPosition.x + sizeTexture, uvPosition.y);
                            }
                            else
                            {
                                //如果已经是最后一个了 则下一排
                                currentPixel += sizeTexture;
                                dicUVPosition[sizeTexture] = new Vector2Int(0, currentPixel);
                                blockModelCreateData.startPixel = new Vector2Int(0, currentPixel);
                            }
                        }
                        else
                        {
                            //如果是第一次添加
                            dicUVPosition.Add(sizeTexture, new Vector2Int(0, currentPixel));
                            blockModelCreateData.startPixel = new Vector2Int(0, currentPixel);
                            currentPixel += sizeTexture;
                        }
                        listCreateData.Add(blockModelCreateData);
                    }
                }
            }

            //是否需要创建TextureArray
            Material matUse = EditorUtil.GetAssetByPath<Material>(pathMatBlock); ;
            //创建方块贴图

            if (textureArrayNumber > 1)
            {
                CreateBlockAnimTexture(blockTextureSize, textureArrayNumber, pathSaveTexure, saveName, listCreateData);
                CreateBlockTextureArray(blockTextureSize, textureArrayNumber, new List<string>() { saveName });
                EditorUtil.RefreshAsset(matUse);
            }
            else
            {
                CreateBlockTexture(blockTextureSize, pathSaveTexure, saveName, 0, listCreateData);
                Texture2D createTex = EditorUtil.GetAssetByPath<Texture2D>($"{pathSaveTexure}/{saveName}_0.png");
                //设置材质球
                matUse.mainTexture = createTex;
                EditorUtil.RefreshAsset(matUse);
            }

            //生成相关模型
            for (int i = 0; i < listCreateData.Count; i++)
            {
                BlockModelCreateBean itemCreateData = listCreateData[i];
                //获取老方块
                GameObject obj = EditorUtil.GetAssetByPath<GameObject>($"{pathRes}/{itemCreateData.nameBlock}.dae");
                MeshFilter objOldMeshFilter = obj.GetComponentInChildren<MeshFilter>();

                GameObject objNew = new GameObject(itemCreateData.nameBlock);
                objNew.transform.localScale = Vector3.one * 0.03125f;
                objNew.transform.localPosition = new Vector3(0, -0.5f, 0);
                MeshRenderer objNewMeshRenderer = objNew.AddComponent<MeshRenderer>();
                MeshFilter objNewMeshFilter = objNew.AddComponent<MeshFilter>();

                //设置UV
                Vector2[] oldUVList = objOldMeshFilter.sharedMesh.uv;
                Vector2[] newUVList = new Vector2[oldUVList.Length];
                for (int f = 0; f < oldUVList.Length; f++)
                {
                    newUVList[f] = oldUVList[f] * (1f / itemCreateData.uvScaleSize);
                    newUVList[f] += itemCreateData.GetStartUV(blockTextureSize);
                }
                //创建新的mesh
                string newMeshName= $"{itemCreateData.nameBlock}_Mesh";
                string pathMesh = $"{Path_Block_Model_Save}/{newMeshName}.asset";

                Mesh newMesh = EditorUtil.GetAssetByPath<Mesh>(pathMesh);
                bool isNew = false;
                if (newMesh == null)
                {
                    isNew = true;
                    newMesh = new Mesh();
                }
                newMesh.name = newMeshName;
                newMesh.SetVertices(objOldMeshFilter.sharedMesh.vertices);
                newMesh.SetTriangles(objOldMeshFilter.sharedMesh.triangles, 0);
                newMesh.SetUVs(0, newUVList);
                newMesh.RecalculateBounds();
                newMesh.RecalculateNormals();
                //保存mesh
                if (isNew)
                {
                    EditorUtil.CreateAsset(newMesh, pathMesh);
                }
                else
                {
                    EditorUtil.SaveAsset(newMesh);
                }
                EditorUtil.RefreshAsset();

                //重新查找这个资源
                newMesh = EditorUtil.GetAssetByPath<Mesh>($"{pathMesh}");
                //设置mesh
                objNewMeshFilter.sharedMesh = newMesh;
                objNewMeshFilter.mesh = newMesh;
                //设置材质
                objNewMeshRenderer.material = matUse;

                EditorUtil.CreatePrefab(objNew, $"{Path_Block_Model_Save}/{itemCreateData.nameBlock}");
                //EditorUtil.RefreshAsset(objNew);
                DestroyImmediate(objNew);
                EditorUtil.RefreshAsset();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

}
