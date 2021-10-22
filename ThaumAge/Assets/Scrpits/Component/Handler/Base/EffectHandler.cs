using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

public class EffectHandler : BaseHandler<EffectHandler, EffectManager>
{
    /// <summary>
    /// 展示粒子特效
    /// </summary>
    /// <param name="objContainer"></param>
    /// <param name="effectData"></param>
    /// <param name="callBack"></param>
    public void ShowEffect(GameObject objContainer,EffectBean effectData,Action<GameObject> callBack = null)
    {
        manager.CreateEffect(objContainer, effectData, (objEffect)=> 
        {
            callBack?.Invoke(objEffect);
            if(effectData.timeForShow > 0) 
            {
                //展示时间过后就删除
                this.WaitExecuteSeconds(effectData.timeForShow, () =>
                {
                    Destroy(objEffect);
                });
            }
        });
    }
}