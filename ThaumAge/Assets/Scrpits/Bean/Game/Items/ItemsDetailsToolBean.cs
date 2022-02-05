using UnityEditor;
using UnityEngine;

public class ItemsDetailsToolBean : ItemsDetailsBean
{
    public int life;//耐久
    public int lifeMax;//最大耐久
    /// <summary>
    /// 添加耐久
    /// </summary>
    public int AddLife(int addLife)
    {
        life += addLife;
        if (life> lifeMax)
        {
            life = lifeMax;
        }
        else if (life<0)
        {
            life = 0;
        }
        return life;
    }
}