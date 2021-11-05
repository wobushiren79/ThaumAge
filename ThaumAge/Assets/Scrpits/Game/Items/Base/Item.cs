using UnityEditor;
using UnityEngine;

public class Item
{
    public ItemsBean itemsData;

    public void SetItemData(ItemsBean itemsData)
    {
        this.itemsData = itemsData;
    }

    /// <summary>
    /// 使用
    /// </summary>
    public virtual void Use()
    {

    }

    /// <summary>
    /// 使用目标
    /// </summary>
    public virtual void UseTarget()
    {

    }

    /// <summary>
    /// 破碎目标
    /// </summary>
    public virtual void BreakTarget(Vector3Int targetPosition)
    {
        //获取原位置方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out BlockTypeEnum oldBlockType, out DirectionEnum oldBlockDirection, out Chunk targetChunk);
        if (targetChunk)
        {
            //如果原位置是空则不做处理
            if (oldBlockType != BlockTypeEnum.None)
            {
                BlockBreak blockBreak = BlockHandler.Instance.BreakBlock(targetPosition, oldBlockType, GetBreakDamage());
                if (blockBreak.blockLife <= 0)
                {
                    //移除破碎效果
                    BlockHandler.Instance.DestroyBreakBlock(targetPosition);
                    //创建掉落物
                    ItemsHandler.Instance.CreateItemDrop(oldBlockType, 1, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
                    //移除该方块
                    targetChunk.RemoveBlockForWorld(targetPosition);
                    WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
                }
                else
                {

                }
            }
        }
    }

    /// <summary>
    /// 获取道具破坏的伤害
    /// </summary>
    public virtual int GetBreakDamage()
    {
        return 1;
    }
}