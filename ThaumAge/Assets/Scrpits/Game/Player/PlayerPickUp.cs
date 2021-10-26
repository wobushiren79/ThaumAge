using UnityEditor;
using UnityEngine;

public class PlayerPickUp : PlayerBase
{
    //拾取速度
    protected float speedPickUp = 1;
    //拾取范围
    protected float rangePickUp = 2;

    public PlayerPickUp(Player player) : base(player)
    {

    }

    /// <summary>
    /// 更新数据
    /// </summary>
    public void UpdatePick()
    {
        HandleForCheckItemDrop();
    }

    /// <summary>
    /// 处理-检测周围掉落道具
    /// </summary>
    public void HandleForCheckItemDrop()
    {
        Collider[] colliderArray = RayUtil.RayToSphere(player.transform.position, rangePickUp, 1 << LayerInfo.Items);
        if (!colliderArray.IsNull())
        {
            for (int i = 0; i < colliderArray.Length; i++)
            {
                Collider itemCollider = colliderArray[i];
                ItemDrop itemDrop = itemCollider.GetComponent<ItemDrop>();
                //如果是丢掉的道具，并且拾取状态为可拾取
                if (itemDrop != null 
                    && itemDrop.GetItemDropState() == ItemDropStateEnum.DropPick)
                {
                    itemDrop.PickItem(player.transform, speedPickUp);
                }
            }
        }
    }
}