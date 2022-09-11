using UnityEditor;
using UnityEngine;

public class ItemsMetaTool : ItemsBaseMeta
{
    public int curDurability;//耐久
    public int durability;//最大耐久
    /// <summary>
    /// 添加耐久
    /// </summary>
    public int AddLife(int addLife)
    {
        curDurability += addLife;
        if (curDurability > durability)
        {
            curDurability = durability;
        }
        else if (curDurability < 0)
        {
            curDurability = 0;
        }
        return curDurability;
    }
}