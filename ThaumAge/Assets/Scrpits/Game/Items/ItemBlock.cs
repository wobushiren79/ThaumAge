using UnityEditor;
using UnityEngine;

public class ItemBlock : Item
{

    public override void Use()
    {
        base.Use();

        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        Player player = GameHandler.Instance.manager.player;
        float disMax = Vector3.Distance(cameraPosition, player.transform.position);
        //发射射线检测
        RayUtil.RayToScreenPointForScreenCenter(disMax + 2, 1 << LayerInfo.Chunk, out bool isCollider, out RaycastHit hit);
        if (isCollider)
        {
            float disHit = Vector3.Distance(cameraPosition, hit.point);
            if (disHit < disMax)
                return;
            Chunk chunkForHit = hit.collider.GetComponentInParent<Chunk>();

            if (chunkForHit)
            {
                //获取位置和方向
                GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out DirectionEnum direction);
                //如果上手没有物品 则挖掘
                if (itemsData == null || itemsData.itemId == 0)
                {
                    //获取原位置方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out BlockTypeEnum oldBlockType, out DirectionEnum oldBlockDirection, out Chunk targetChunk);
                    if (targetChunk)
                    {
                        //如果原位置是空则不做处理
                        if (oldBlockType != BlockTypeEnum.None)
                        {
                            //创建掉落物
                            ItemsHandler.Instance.CreateItemDrop(oldBlockType, 1, targetPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);

                            targetChunk.RemoveBlockForWorld(targetPosition);
                            WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
                        }
                    }
                    //显示位置
                    //GameHandler.Instance.manager.playerTargetBlock.Show(targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    //首先获取靠近方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out BlockTypeEnum block, out DirectionEnum blockDirection, out Chunk addChunk);
                    //如果靠近得方块有区块
                    if (addChunk)
                    {
                        //获取物品信息
                        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
                        ItemsTypeEnum itemsType = itemsInfo.GetItemsType();
                        //如果是可放置的方块
                        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(itemsInfo.type_id);
                        //更新方块并 添加更新区块
                        if (blockInfo.rotate_state == 0)
                        {
                            addChunk.SetBlockForWorld(closePosition, blockInfo.GetBlockType(), DirectionEnum.UP);
                        }
                        else
                        {
                            addChunk.SetBlockForWorld(closePosition, blockInfo.GetBlockType(), direction);
                        }
                        //更新区块
                        WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
                    }
                    //显示位置
                    //GameHandler.Instance.manager.playerTargetBlock.Show(closePosition);
                }
            }
        }
        else
        {
            GameHandler.Instance.manager.playerTargetBlock.Hide();
        }
    }

    /// <summary>
    /// 获取碰撞的位置和方向
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetPosition"></param>
    /// <param name="closePosition"></param>
    /// <param name="direction"></param>
    protected void GetHitPositionAndDirection(RaycastHit hit, out Vector3Int targetPosition, out Vector3Int closePosition, out DirectionEnum direction)
    {
        targetPosition = Vector3Int.zero;
        closePosition = Vector3Int.zero;
        direction = DirectionEnum.UP;
        if (hit.normal.y > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y) - 1, (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.up;
            direction = DirectionEnum.UP;
        }
        else if (hit.normal.y < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.down;
            direction = DirectionEnum.Down;
        }
        else if (hit.normal.x > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x) - 1, (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.right;
            direction = DirectionEnum.Right;
        }
        else if (hit.normal.x < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.left;
            direction = DirectionEnum.Left;
        }
        else if (hit.normal.z > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z) - 1);
            closePosition = targetPosition + Vector3Int.forward;
            direction = DirectionEnum.Forward;
        }
        else if (hit.normal.z < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.back;
            direction = DirectionEnum.Back;
        }
    }

}