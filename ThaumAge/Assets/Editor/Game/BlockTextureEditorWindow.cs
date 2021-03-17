using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BlockTextureEditorWindow : EditorWindow
{

    [MenuItem("工具/方块图片生成工具")]
    static void CreateWindows()
    {
        EditorWindow.GetWindow(typeof(BlockTextureEditorWindow));
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        if (EditorUI.GUIButton("生成方块图片", 150))
        {
            CreateBlockTexture(2048);
        }
        EditorUI.GUIPic("Assets/Texture/", "block", 512, 512);
        GUILayout.EndVertical();
    }

    public void CreateBlockTexture(int size)
    {
        string path = "Assets/Texture/Block";
        string[] filesName = Directory.GetFiles(path);
        Texture2D outTexture = new Texture2D(size, size, TextureFormat.RGBA32, true);
        int itemSize = size / 128;
        for (int i = 0; i < filesName.Length; i++)
        {
            string fileName = filesName[i];
            if (fileName.Contains(".meta"))
            {
                continue;
            }
            Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>(fileName);
            string[] itemDataArray = StringUtil.SplitBySubstringForArrayStr(itemTex.name, '_');
            int positionStartX = int.Parse(itemDataArray[1]) * itemSize;
            int positionStartY = int.Parse(itemDataArray[0]) * itemSize;


            int width = itemTex.width;
            int height = itemTex.height;
            outTexture.SetPixels(positionStartX, positionStartY, width, height, itemTex.GetPixels());
        }


        //保存图片
        File.WriteAllBytes("Assets/Texture/block.png", outTexture.EncodeToPNG());
        AssetDatabase.Refresh();
    }


}
