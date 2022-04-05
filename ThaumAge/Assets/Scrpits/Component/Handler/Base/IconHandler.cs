using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class IconHandler : BaseHandler<IconHandler, IconManager>
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

    /// <summary>
    /// 获取图标sprite
    /// </summary>
    /// <param name="spriteData">前图集 后名字 用,分割</param>
    public void GetIconSprite(string spriteData, Action<Sprite> callBack)
    {
        string[] spriteArrayData = spriteData.SplitForArrayStr(',');

        Action<Sprite> callBackForComplete = (sprite) =>
        {
            if (sprite == null)
            {
                GetUnKnowSprite(callBack);
            }
            else
            {
                callBack?.Invoke(sprite);
            }
        };
        if (spriteArrayData[0].Equals("SpriteAtlasForUI"))
        {
            manager.GetUISpriteByName(spriteArrayData[1], callBackForComplete);
        }
        else if (spriteArrayData[0].Equals("SpriteAtlasForItems"))
        {
            manager.GetItemsSpriteByName(spriteArrayData[1], callBackForComplete);
        }
    }
}