using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureHandler : BaseHandler<CreatureHandler, CreatureManager>
{
    public List<CreatureCptBase> listCreature = new List<CreatureCptBase>();

    /// <summary>
    /// 创建生物
    /// </summary>
    public void CreateCreature(long creatureId, Vector3 position, Action<CreatureCptBase> callBackForComplete = null)
    {
        CreatureInfoBean creatureInfo = CreatureInfoCfg.GetItemData(creatureId);
        if (creatureInfo == null)
            return;
        manager.GetCreatureModel(creatureInfo.model_name, (data) =>
        {
            //创建生物
            GameObject objCreature = Instantiate(gameObject, data);
            //设置生物位置
            objCreature.transform.position = position;
            //获取生物组件
            CreatureCptBase creatureCpt = objCreature.GetComponent<CreatureCptBase>();
            //设置生物信息
            creatureCpt.SetData(creatureInfo);
            //回调
            callBackForComplete?.Invoke(creatureCpt);
            //添加到列表里
            listCreature.Add(creatureCpt);
        });
    }


    /// <summary>
    /// 创建生物血条
    /// </summary>
    public CreatureCptLifeProgress CreateCreatureLifeProgress(GameObject creatureObj)
    {
        //获取模型
        GameObject objLifeProgressModel = manager.GetCreatureLifeProgressModel();
        //实例化
        GameObject objLifeProgress = Instantiate(creatureObj, objLifeProgressModel);
        //获取控件
        CreatureCptLifeProgress creatureCptLife = objLifeProgress.GetComponent<CreatureCptLifeProgress>();
        //设置位置
        objLifeProgress.transform.localPosition = new Vector3(0, 1.5f, 0);
        return creatureCptLife;
    }

    /// <summary>
    /// 删除生物
    /// </summary>
    /// <param name="creatureCpt"></param>
    public void DestoryCreature(CreatureCptBase creatureCpt)
    {
        listCreature.Remove(creatureCpt);
        Destroy(creatureCpt.gameObject);
    }
}