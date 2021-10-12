using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class IconHandler : BaseHandler<IconHandler,IconManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void InitData()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    public static void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
    {
        SpriteAtlas sa = LoadAssetUtil.SyncLoadAsset<SpriteAtlas>("sprite/atlas", tag);
        //SpriteAtlas sa = LoadResourcesUtil.AsyncLoadData<SpriteAtlas>("SpriteAtlas/" + tag);
        if (sa != null)
            callback?.Invoke(sa);
    }

    /// <summary>
    /// 获取未知图标
    /// </summary>
    /// <returns></returns>
    public Sprite GetUnKnowSprite()
    {
        return manager.GetItemsSpriteByName("icon_unknow");
    }
}