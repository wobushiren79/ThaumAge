using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CreatureBean
{
    //最大生命值
    public int maxLife;
    //当前生命值
    public int currentLife;

    /// <summary>
    /// 增加生命值
    /// </summary>
    /// <param name="addLife"></param>
    /// <returns></returns>
    public int AddLife(int addLife)
    {
        currentLife += addLife;
        if (currentLife < 0)
        {
            currentLife = 0;
        }
        if (currentLife > maxLife)
        {
            currentLife = maxLife;
        }
        return currentLife;
    }
}