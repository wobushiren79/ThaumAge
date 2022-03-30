using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class IconHandler : BaseHandler<IconHandler,IconManager>
{
    public override void Awake()
    {
        base.Awake();
    }

    public void InitData()
    {
        SpriteAtlasManager.atlasRequested += RequestAtlas;
    }

    public void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
    {
        Action<AsyncOperationHandle<SpriteAtlas>> loadCallBack = (data) =>
        {
            if (data.Result != null)
            {
                SpriteAtlas spriteAtlas = data.Result;
                if (spriteAtlas != null)
                    callback?.Invoke(spriteAtlas);
            }
        };
        LoadAddressablesUtil.LoadAssetAsync(manager.PathSpriteAtlasForUI, loadCallBack);
        LoadAddressablesUtil.LoadAssetAsync(manager.PathSpriteAtlasForItems, loadCallBack);
    }

    /// <summary>
    /// 获取未知图标
    /// </summary>
    /// <returns></returns>
    public void GetUnKnowSprite(Action<Sprite> callBack)
    {
        manager.GetItemsSpriteByName("icon_unknow", callBack);
    }
}