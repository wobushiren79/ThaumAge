using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;

public class ResourcesRefresh : Editor
{
    //方块动画帧数
    public static int BlockAnimFrameNumber = 10;
    //方块动画速度
    public static float BlockAnimSpeed = 1;

    [MenuItem("工具/资源/刷新所有资源")]
    public static void RefreshAllRes()
    {
        try
        {
            //生成所有自定义方块数据
            RefreshBlockModelCustomRes();
            LogUtil.Log("生成所有自定义方块数据 完毕");
            // 生成所有通用方块数据
            RefreshBlockModelCommonRes();
            LogUtil.Log("生成所有通用方块数据 完毕");
            //生成动画贴图
            RefreshBlockAnimTexture();
            LogUtil.Log("生成动画贴图 完毕");
            //生成TextureArray资源
            RefreshBlockTextureArray();
            LogUtil.Log("生成TextureArray资源 完毕");
            //刷新贴图材质
            RefreshBlockMat();
            LogUtil.Log("刷新贴图材质 完毕");
            LogUtil.Log("刷新所有资源 完毕");
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

    [MenuItem("工具/资源/刷新TextureArray贴图")]
    public static void RefreshBlockAnimTexture()
    {
        try
        {
            //生成单个的预览贴图
            BlockEditorWindow.CreateBlockAnimTexture
                (
                2048,
                BlockAnimFrameNumber + 1,
                BlockEditorWindow.Path_Block_Textures,
                new List<string>() {
                    "Block" + BlockMaterialEnum.Normal.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFace.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFaceSwing.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFaceSwingUniform.GetEnumName(),
                    "Block" + BlockMaterialEnum.Transparent.GetEnumName(),
                }
                );
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

    [MenuItem("工具/资源/刷新TextureArray")]
    public static void RefreshBlockTextureArray()
    {
        try
        {
            //生成单个的预览贴图
            BlockEditorWindow.CreateBlockTextureArray
                (
                2048,
                BlockAnimFrameNumber + 1,
                new List<string>() {
                    "Block"+ BlockMaterialEnum.Normal.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFace.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFaceSwing.GetEnumName(),
                    "Block" + BlockMaterialEnum.BothFaceSwingUniform.GetEnumName(),
                    "Block" + BlockMaterialEnum.Transparent.GetEnumName()}
                );
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

    [MenuItem("工具/资源/刷新设置方块材质")]
    public static void RefreshBlockMat()
    {
        List<BlockMaterialEnum> listBlockMat = new List<BlockMaterialEnum>() {
            BlockMaterialEnum.Custom,
            BlockMaterialEnum.Normal,
            BlockMaterialEnum.BothFace,
            BlockMaterialEnum.BothFaceSwing,
            BlockMaterialEnum.BothFaceSwingUniform,
            BlockMaterialEnum.Transparent
        };
        for (int i = 0; i < listBlockMat.Count; i++)
        {
            string matOfName = "Block" + listBlockMat[i].GetEnumName();
            int matOfIndex = (int)listBlockMat[i];

            Material blockMat = AssetDatabase.LoadAssetAtPath<Material>($"{BlockEditorWindow.Path_Prefabs_BlockMat}/{matOfName}_{matOfIndex}.mat");
            Texture2DArray blockTexture = AssetDatabase.LoadAssetAtPath<Texture2DArray>($"{BlockEditorWindow.Path_BlockTextureArray}/BlockTextureArrary_{matOfName}.asset");
            blockMat.SetTexture("_BlockTextureArray", blockTexture);
            blockMat.SetFloat("_AnimSpeed", BlockAnimSpeed);
            blockMat.SetFloat("_AnimLength", BlockAnimFrameNumber + 1);
        }
        EditorUtil.RefreshAsset();
    }

    [MenuItem("工具/资源/刷新自定义方块模型（合并的模型）")]
    public static void RefreshBlockModelCustomRes()
    {
        //注： 如果是超过2048的图片 需要选用其他的压缩格式
        BlockEditorWindow.CreateBlockModel(2048, BlockEditorWindow.Path_FBX_BlockModelCustom, $"{BlockEditorWindow.Path_BlockTexturesMat}", "BlockCustom", BlockEditorWindow.Path_BlockMatCustom);
        BlockEditorWindow.CreateBlockMeshData();
    }

    [MenuItem("工具/资源/刷新通用方块模型(单独的模型)")]
    public static void RefreshBlockModelCommonRes()
    {
        //注： 如果是超过2048的图片 需要选用其他的压缩格式
        BlockEditorWindow.CreateBlockModel(2048, BlockEditorWindow.Path_FBX_BlockModelCommon, $"{BlockEditorWindow.Path_BlockTexturesMat}", "BlockCommon", BlockEditorWindow.Path_BlockMatCommon);
    }

    [MenuItem("工具/资源/刷新FBX资源（装备）")]
    public static void RefreshFBXForEquip()
    {
        string equipPath = "Assets/Art/FBX/Equip";
        string equipModelPath = "Assets/Prefabs/Model/Character/Equip";
        CreateFBXMatAndSet(equipPath);
        CreateModelGameObject(equipPath, equipModelPath);
        EditorUtil.RefreshAsset();
    }

    [MenuItem("工具/资源/刷新FBX资源（发型）")]
    public static void RefreshFBXForHair()
    {
        string hairPath = "Assets/Art/FBX/Character/Hair";
        string hairModelPath = "Assets/Prefabs/Model/Character/Hair";
        Material objMat = EditorUtil.GetAssetByPath<Material>($"{hairPath}/Hair_Mat_1.mat");
        CreateModelGameObject(hairPath, hairModelPath, objMat);
        EditorUtil.RefreshAsset();
    }

    [MenuItem("工具/资源/刷新FBX资源（生物）")]
    public static void RefreshFBXForCreature()
    {
        string creaturePath = "Assets/Art/FBX/Creature";
        CreateFBXMatAndSet(creaturePath);
        EditorUtil.RefreshAsset();
    }

    [MenuItem("工具/资源/刷新动画资源（生物）")]
    public static void RefreshFBXAnimForCreature()
    {
        string creatureAnimPath = "Assets/Anim/Creature";
        //获取文件价目录下的所有文件
        FileInfo[] fileInfos = FileUtil.GetFilesByPath(creatureAnimPath);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo file = fileInfos[i];
            if (file.Name.Contains(".meta"))
                continue;
            if (!file.Name.Contains(".fbx"))
                continue;
            string fbxPaht = $"{creatureAnimPath}/{file.Name}";
            FBXEditor.ChangeAnim(fbxPaht, isLoop: true);
        }
        EditorUtil.RefreshAsset();
    }



    protected static void CreateFBXMatAndSet(string fbxFilesPath)
    {
        //获取文件价目录下的所有文件
        FileInfo[] fileInfos = FileUtil.GetFilesByPath(fbxFilesPath);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo file = fileInfos[i];

            if (file.Name.Contains(".meta"))
                continue;
            if (file.Name.Contains("_texture0"))
            {
                //设置贴图        
                EditorUtil.SetTextureData($"{fbxFilesPath}/{file.Name}", isReadable: true, mipmapEnabled: true, textureImporterCompression: TextureImporterCompression.Uncompressed);
                continue;
            }

            if (file.Name.Contains("_Mat"))
                continue;

            string fileName = file.Name.Replace(".dae", "").Replace(".fbx", "");
            string texturePath = $"{fbxFilesPath}/{fileName}_texture0.png";
            string matCreatePath = $"{fbxFilesPath}/{fileName}_Mat";
            string matCreateAllPath = $"{matCreatePath}.mat";

            //GameObject obj = EditorUtil.GetAssetByPath<GameObject>($"{equipPath}/{file.Name}");

            //判断是否有对应的材质
            Material objMat = EditorUtil.GetAssetByPath<Material>($"{matCreateAllPath}");
            if (objMat == null)
            {
                EditorUtil.CreateMaterial(texturePath, "HDRP/Lit", matCreatePath);
            }

            FBXEditor.ChangeMaterial($"{fbxFilesPath}/{file.Name}", $"{matCreateAllPath}");
        }
    }

    protected static void CreateModelGameObject(string sourcePath, string createPath, Material targetMat = null)
    {

        //获取文件价目录下的所有文件
        FileInfo[] fileInfos = FileUtil.GetFilesByPath(sourcePath);
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo file = fileInfos[i];
            if (file.Name.Contains(".meta"))
                continue;
            if (!file.Name.Contains(".fbx") && !file.Name.Contains(".dae"))
                continue;
            string objName = file.Name.Replace(".fbx", "").Replace(".dae", "");
            string createObjPath = $"{createPath}/{objName}.prefab";
            //如果已经有该obj 则不创建了
            GameObject objCreate = EditorUtil.GetAssetByPath<GameObject>(createObjPath);

            if (objCreate != null)
                continue;

            //设置OBJ
            objCreate = new GameObject(objName);
            GameObject objModel = new GameObject("Model");
            objModel.transform.parent = objCreate.transform;

            GameObject objFBXModel = EditorUtil.GetAssetByPath<GameObject>($"{sourcePath}/{file.Name}");
            GameObject objFBX = Instantiate(objFBXModel);
            objFBX.name = $"{objName}FBX";
            objFBX.transform.localEulerAngles = new Vector3(0, 0, 0);
            objFBX.transform.localScale = new Vector3(0.03125f, 0.03125f, 0.03125f);
            objFBX.transform.localPosition = new Vector3(0, 0, 0);
            objFBX.transform.parent = objModel.transform;

            if (targetMat != null)
            {
                MeshRenderer meshRenderer = objFBX.GetComponentInChildren<MeshRenderer>();
                meshRenderer.material = targetMat;
            }

            //创建预支体
            EditorUtil.CreatePrefab(objCreate, $"{createPath}/{objName}");

            //加上Address
            AddressableAssetGroup addressableAssetGroup = AddressableUtil.FindOrCreateGroup("Equip");
            AddressableUtil.AddAssetEntry(addressableAssetGroup, createObjPath, createObjPath);

            DestroyImmediate(objCreate);
        }
    }
}