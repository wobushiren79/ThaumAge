using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicTypeBase
{
    public MagicBean magicData;
    public List<GameObject> listMagicObj = new List<GameObject>();

    //存在生命周期
    protected int timeForLife = 0;

    /// <summary>
    /// 设置数据
    /// </summary>
    public virtual void SetData(MagicBean magicData)
    {
        this.magicData = magicData;
    }

    /// <summary>
    /// 更新魔法
    /// </summary>
    public virtual void UpdateMagic()
    {
        timeForLife++;
        if (timeForLife >= magicData.lifeTime)
        {
            timeForLife = 0;
            DestoryMagic();
        }
    }

    /// <summary>
    /// 删除当前魔法
    /// </summary>
    public virtual void DestoryMagic()
    {
        for (int i = 0; i < listMagicObj.Count; i++)
        {
            GameObject objMagicCpt = listMagicObj[i];
            GameObject.Destroy(objMagicCpt);
        }
        listMagicObj.Clear();
        MagicHandler.Instance.DestoryMagic(this);
    }

    /// <summary>
    /// 魔法碰撞检测
    /// </summary>
    /// <param name="collider"></param>
    public virtual void CallBackForMagicTriggerEnter(MagicCpt magicCpt, Collider collider)
    {
        if (collider == null)
            return;

        int colliderLayer = collider.gameObject.layer;
        if (colliderLayer == LayerInfo.ChunkCollider)
        {
            HandleForTriggerBlock(magicCpt, collider);
        }
        else if (colliderLayer == LayerInfo.Items)
        {
            //暂时不处理
            HandleForTriggerItem(magicCpt, collider);
        }
        else if (colliderLayer == LayerInfo.Magic)
        { 
            //暂时不处理
            HandleForTriggerMagic(magicCpt, collider);
        }
        else if (colliderLayer == LayerInfo.Character || colliderLayer == LayerInfo.Creature)
        {
            HandleForTriggerCreature( magicCpt,  collider);
        }
        DestoryMagic();
    }

    /// <summary>
    /// 处理方块碰撞（默认摧毁）
    /// </summary>
    /// <param name="collider"></param>
    public virtual void HandleForTriggerBlock(MagicCpt magicCpt, Collider collider)
    {


    }

    /// <summary>
    /// 处理和道具碰撞（默认摧毁）
    /// </summary>
    /// <param name="magicCpt"></param>
    /// <param name="collider"></param>
    public virtual void HandleForTriggerItem(MagicCpt magicCpt, Collider collider)
    {

    }

    /// <summary>
    /// 处理和魔法碰撞（默认摧毁）
    /// </summary>
    /// <param name="magicCpt"></param>
    /// <param name="collider"></param>
    public virtual void HandleForTriggerMagic(MagicCpt magicCpt, Collider collider)
    {

    }

    /// <summary>
    /// 处理和生物的碰撞（默认摧毁）
    /// </summary>
    /// <param name="magicCpt"></param>
    /// <param name="collider"></param>
    public virtual void HandleForTriggerCreature(MagicCpt magicCpt, Collider collider)
    {

    }
}