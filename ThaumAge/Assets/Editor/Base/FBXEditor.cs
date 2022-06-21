using System.IO;
using UnityEditor;
using UnityEngine;

public class FBXEditor : Editor
{

    /// <summary>
    /// 修改材质
    /// </summary>
    /// <param name="FBXPath">FBX的路径</param>
    /// <param name="expectedMaterialPath">替换材质的路径</param>
    public static void ChangeMaterial(string FBXPath, string expectedMaterialPath)
    {
        if (FBXPath.Contains(".meta"))
            return;
        AssetImporter assetImporter = AssetImporter.GetAtPath(FBXPath);
        if (assetImporter == null)
            return;
        ModelImporter modelImporter = assetImporter as ModelImporter;
        if (!modelImporter)
            return;
        
        //获取替换的材质
        Material expectedMaterial= EditorUtil.GetAssetByPath<Material>(expectedMaterialPath);
        if (expectedMaterial == null)
            return;

        //保存数据
        SerializedObject modelImporterObj = new SerializedObject(modelImporter);

        var materials = modelImporterObj.FindProperty("m_Materials");
        var externalObjects = modelImporterObj.FindProperty("m_ExternalObjects");

        for (int materialIndex = 0; materialIndex < materials.arraySize; materialIndex++)
        {
            var id = materials.GetArrayElementAtIndex(materialIndex);
            var name = id.FindPropertyRelative("name").stringValue;
            var type = id.FindPropertyRelative("type").stringValue;
            var assembly = id.FindPropertyRelative("assembly").stringValue;

            SerializedProperty materialProperty = null;

            for (int externalObjectIndex = 0; externalObjectIndex < externalObjects.arraySize; externalObjectIndex++)
            {
                var currentSerializedProperty = externalObjects.GetArrayElementAtIndex(externalObjectIndex);
                var externalName = currentSerializedProperty.FindPropertyRelative("first.name").stringValue;
                var externalType = currentSerializedProperty.FindPropertyRelative("first.type").stringValue;

                if (externalType == type && externalName == name)
                {
                    materialProperty = currentSerializedProperty.FindPropertyRelative("second");
                    break;
                }
            }

            if (materialProperty == null)
            {
                var lastIndex = externalObjects.arraySize++;
                var currentSerializedProperty = externalObjects.GetArrayElementAtIndex(lastIndex);
                currentSerializedProperty.FindPropertyRelative("first.name").stringValue = name;
                currentSerializedProperty.FindPropertyRelative("first.type").stringValue = type;
                currentSerializedProperty.FindPropertyRelative("first.assembly").stringValue = assembly;
                currentSerializedProperty.FindPropertyRelative("second").objectReferenceValue = expectedMaterial;
            }
            else
            {
                materialProperty.objectReferenceValue = expectedMaterial;
            }
        }
        modelImporterObj.ApplyModifiedPropertiesWithoutUndo();
        modelImporterObj.ApplyModifiedProperties();

        modelImporter.SaveAndReimport();
        AssetDatabase.ImportAsset(FBXPath);
    }
}