using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSaveCreatureBean : ChunkSaveBaseBean
{
    public List<CreatureBean> listCreatureData = new List<CreatureBean>();

    /// <summary>
    /// 增加生物信息
    /// </summary>
    /// <param name="targetCreatureData"></param>
    public void AddCreatureData(CreatureBean targetCreatureData)
    {
        RemoveCreatureData(targetCreatureData);
        listCreatureData.Add(targetCreatureData);
    }

    /// <summary>
    /// 移除生物信息
    /// </summary>
    /// <param name="targetCreatureData"></param>
    public void RemoveCreatureData(CreatureBean targetCreatureData)
    {
        for (int i = 0; i < listCreatureData.Count; i++)
        {
            var itemCreatureData = listCreatureData[i];
            if (itemCreatureData.creatureId.Equals(targetCreatureData.creatureId))
            {
                listCreatureData.Remove(itemCreatureData);
                break;
            }
        }
    }
}