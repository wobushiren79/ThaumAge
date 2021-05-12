using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class IconManager : BaseManager
{
    //UI图标
    public SpriteAtlas AtlasForUI;
    public SpriteAtlas AtlasForItems;

    public IconBeanDictionary dicUI = new IconBeanDictionary();
    public IconBeanDictionary dicItems = new IconBeanDictionary();

    public Dictionary<string, Texture2D> dicTextureUI = new Dictionary<string, Texture2D>();
    
    /// <summary>
    /// 根据名字获取UI图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetUISpriteByName(string name)
    {
        return GetSpriteByName(dicUI, ref AtlasForUI, "SpriteAtlasForUI", "sprite/atlas", name);
    }

    /// <summary>
    /// 根据名字获取物品图标
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemsSpriteByName(string name)
    {
        return GetSpriteByName(dicItems, ref AtlasForItems, "SpriteAtlasForItem", "sprite/atlas", name);
    }

    public Texture2D GetTextureUIByName(string name)
    {
        return GetModel(dicTextureUI, "texture/ui", name);
    }
}