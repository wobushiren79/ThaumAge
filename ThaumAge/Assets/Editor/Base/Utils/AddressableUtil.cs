using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressableUtil
{
    /// <summary>
    ///创建分组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public static AddressableAssetGroup FindOrCreateGroup(string groupName)
    {
        AddressableAssetSettings Settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup group = Settings.FindGroup(groupName);
        if (group == null)
            group = Settings.CreateGroup(groupName, false, false, false, null);
        Settings.AddLabel(groupName, false);
        return group;
    }

    /// <summary>
    /// 给某分组添加资源
    /// </summary>
    /// <param name="group"></param>
    /// <param name="assetPath"></param>
    /// <param name="address"></param>
    /// <returns></returns>
    public static AddressableAssetEntry AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
    {
        string guid = AssetDatabase.AssetPathToGUID(assetPath);
        AddressableAssetSettings Settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetEntry entry = group.entries.FirstOrDefault(e => e.guid == guid);
        if (entry == null)
        {
            entry = Settings.CreateOrMoveEntry(guid, group, false, false);
        }

        entry.address = address;
        entry.SetLabel(group.Name, true, false, false);
        return entry;
    }
}