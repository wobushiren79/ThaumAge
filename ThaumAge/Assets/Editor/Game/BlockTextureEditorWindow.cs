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
        EditorUI.GUIPic("Assets/Texture/", "block", 512, 512);
        if (EditorUI.GUIButton("生成方块图片", 150))
        {
            CreateBlockTexture();
        }
        GUILayout.EndVertical();
    }

    public void CreateBlockTexture()
    {
        string path = "Assets/Texture/Block";
        string[] filesName = Directory.GetFiles(path);
        for (int i = 0; i < filesName.Length; i++)
        {
            string fileName = filesName[i];
            Texture2D itemTex = AssetDatabase.LoadAssetAtPath<Texture2D>(fileName);
            //http://www.xuanyusong.com/archives/4729
        }
    }

}
