using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

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
                new List<string>() { "Block" + BlockMaterialEnum.Normal.GetEnumName(), "Block" + BlockMaterialEnum.BothFace.GetEnumName(), "Block" + BlockMaterialEnum.BothFaceSwing.GetEnumName(), "Block" + BlockMaterialEnum.BothFaceSwingUniform.GetEnumName() }
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
                new List<string>() { "Block"+ BlockMaterialEnum.Normal.GetEnumName(), "Block" + BlockMaterialEnum.BothFace.GetEnumName(), "Block" + BlockMaterialEnum.BothFaceSwing.GetEnumName(), "Block" + BlockMaterialEnum.BothFaceSwingUniform.GetEnumName() }
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
        List<BlockMaterialEnum> listBlockMat = new List<BlockMaterialEnum>() { BlockMaterialEnum.Custom, BlockMaterialEnum.Normal, BlockMaterialEnum.BothFace, BlockMaterialEnum.BothFaceSwing, BlockMaterialEnum.BothFaceSwingUniform };
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
        BlockEditorWindow.CreateBlockModel(2048, BlockEditorWindow.Path_FBX_BlockModelCustom, $"{BlockEditorWindow.Path_BlockTexturesMat}", "BlockCustom", BlockEditorWindow.Path_BlockMatCustom, BlockAnimFrameNumber + 1);
        BlockEditorWindow.CreateBlockMeshData();
    }

    [MenuItem("工具/资源/刷新通用方块模型(单独的模型)")]
    public static void RefreshBlockModelCommonRes()
    {
        //注： 如果是超过2048的图片 需要选用其他的压缩格式
        BlockEditorWindow.CreateBlockModel(2048, BlockEditorWindow.Path_FBX_BlockModelCommon, $"{BlockEditorWindow.Path_BlockTexturesMat}", "BlockCommon", BlockEditorWindow.Path_BlockMatCommon);
    }
}