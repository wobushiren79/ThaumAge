using System;
using UnityEditor;
using UnityEngine;

public class GameHandler : BaseHandler<GameHandler, GameManager>
{
    public void LoadGameResources(Action callBack)
    {       
        //加载世界资源
        WorldCreateHandler.Instance.manager.LoadResources(() => 
        {
            //加载方块资源
            BlockHandler.Instance.manager.LoadResources(() => 
            {
                callBack?.Invoke();
            });
        });
    }

}