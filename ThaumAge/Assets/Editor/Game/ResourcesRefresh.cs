using System.IO;
using UnityEditor;
using UnityEngine;

public class ResourcesRefresh : Editor
{
    protected static readonly string Path_Prefabs_BlockMat = "Assets/Prefabs/Mats";
    protected static readonly string Path_Texture_BlockTextureArray = "Assets/Texture/BlockTextureArray";

    //方块动画帧数
    public static int BlockAnimFrameNumber = 10;
    //方块动画速度
    public static float BlockAnimSpeed = 1;

    [MenuItem("工具/资源/刷新所有资源")]
    public static void RefreshAllRes()
    {
        try
        {
            //生成贴图
            RefreshTextureArrayTexture();
            //生成图集资源
            RefreshTextureArray();
            //设置图集资源
            RefreshBlockMat();
            //生成所有自定义方块的mesh数据
            BlockEditorWindow.CreateBlockMeshData();
            //首先生成各种方块贴图 和对应的arrayTextures 然后赋值到对应的材质球上
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }
    [MenuItem("工具/资源/刷新TextureArray贴图")]
    public static void RefreshTextureArrayTexture()
    {
        try
        {
            //生成单个的预览贴图
            BlockEditorWindow.CreateBlockTextureArrayTexture(2048, BlockAnimFrameNumber + 1);
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

    [MenuItem("工具/资源/刷新TextureArray")]
    public static void RefreshTextureArray()
    {
        try
        {
            //生成单个的预览贴图
            BlockEditorWindow.CreateBlockTextureArray(2048, BlockAnimFrameNumber + 1);
        }
        finally
        {
            EditorUI.GUIHideProgressBar();
        }
    }

    [MenuItem("工具/资源/刷新设置方块材质")]
    public static void RefreshBlockMat()
    {
        for (int i = 0; i < BlockEditorWindow.listBlockMat.Count; i++)
        {
            string matOfName = BlockEditorWindow.listBlockMat[i].GetEnumName();
            int matOfIndex = (int)BlockEditorWindow.listBlockMat[i];

            Material blockMat = AssetDatabase.LoadAssetAtPath<Material>($"{Path_Prefabs_BlockMat}/BlockMat{matOfName}_{matOfIndex}.mat");
            Texture2DArray blockTexture = AssetDatabase.LoadAssetAtPath<Texture2DArray>($"{Path_Texture_BlockTextureArray}/BlockTextureArrary_{matOfName}.asset");
            blockMat.SetTexture("_BlockTextureArray", blockTexture);
            blockMat.SetFloat("_AnimSpeed", BlockAnimSpeed);
            blockMat.SetFloat("_AnimLength", BlockAnimFrameNumber + 1);
        }
        EditorUtil.RefreshAsset();
    }


    [MenuItem("工具/资源/刷新方块模型")]
    public static void RefreshBlockModelRes()
    {
        BlockEditorWindow.CreateBlockModel();
    }
}