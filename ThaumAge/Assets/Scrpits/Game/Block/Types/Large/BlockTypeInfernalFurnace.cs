using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeInfernalFurnace : BlockBaseLinkLarge, IBlockForItemsPutOut
{
    public override BuildingTypeEnum GetBuildingType()
    {
        return BuildingTypeEnum.InfernalFurnace;
    }

    public override void InitBlockColor(Color[] colorArray)
    {
        Color lightColor = new Color(0.63f, 0.023f, 0, 2);
        BlockShapeCustom blockShapeCustom = blockShape as BlockShapeCustom;
        for (int i = 0; i < colorArray.Length; i++)
        {
            MeshDataCustom meshDataCustom = blockShapeCustom.GetBlockMeshData();
            Color texColor = meshDataCustom.mainMeshData.texColor[i];
            if (texColor.r > 0.5f)
            {
                colorArray[i] = lightColor;
            }
            else
            {
                colorArray[i] = Color.white;
            }
        }
    }


    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        //刷新的时候注册事件 
        if (state == 0 || state == 1)
            StartWork(chunk, localPosition);
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public void StartWork(Chunk chunk, Vector3Int localPosition)
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 获取火焰加成
    /// </summary>
    public virtual int GetTransitionSpeed(Chunk chunk, Vector3Int localPosition)
    {
        Vector3Int upPosition = localPosition + Vector3Int.up;
        BlockDirectionEnum blockDirection = chunk.chunkData.GetBlockDirection(localPosition);
        blockShape.GetCloseRotateBlockByDirection(chunk, upPosition, blockDirection, DirectionEnum.Left, out Block leftBlock, out Chunk leftChunk, out Vector3Int leftLocalPosition, 2);
        blockShape.GetCloseRotateBlockByDirection(chunk, upPosition, blockDirection, DirectionEnum.Right, out Block rightBlock, out Chunk rightChunk, out Vector3Int rightLocalPosition, 2);
        blockShape.GetCloseRotateBlockByDirection(chunk, upPosition, blockDirection, DirectionEnum.Back, out Block backBlock, out Chunk backChunk, out Vector3Int backLocalPosition, 2);

        int addData = 1;
        if (leftChunk != null && leftBlock != null && leftBlock.blockType == BlockTypeEnum.ArcaneBellows)
        {
            addData++;
        }
        if (rightChunk != null && rightBlock != null && rightBlock.blockType == BlockTypeEnum.ArcaneBellows)
        {
            addData++;
        }
        if (backChunk != null && backBlock != null && backBlock.blockType == BlockTypeEnum.ArcaneBellows)
        {
            addData++;
        }
        return addData;
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        //获取数据
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfernalFurnace blockMetaData);

        bool isSaveData = true;
        //如果没有烧制的物品 则结束
        if (blockMetaData.listItem.IsNull())
        {
            isSaveData = false;
        }
        else
        {
            long itemBeforeId = blockMetaData.listItem[0];
            //查询烧制之前的物品能否烧制物品
            ItemsInfoBean itemsInfoBefore = ItemsHandler.Instance.manager.GetItemsInfoById(itemBeforeId);
            if (itemsInfoBefore.fire_items.IsNull())
            {
                //如果是不能烧制的物品 则删除这个物品
                blockMetaData.listItem.RemoveAt(0);
            }
            else
            {
                //获取烧制的结果
                itemsInfoBefore.GetFireItems(out int[] fireItemsId, out int[] fireItemsNum, out int[] fireTime);
                int itemFireTime = fireTime[0];
                float transitionSpeed = GetTransitionSpeed(chunk, localPosition);
                //检测是否正在烧制物品
                if (blockMetaData.transitionPro < 1)
                {
                    blockMetaData.transitionPro += transitionSpeed / itemFireTime;
                }
                else if (blockMetaData.transitionPro >= 1)
                {
                    //烧制完成
                    blockMetaData.transitionPro = 0;
                    blockMetaData.listItem.RemoveAt(0);
                    //吐出物品
                    Vector3 worldPositionForOut = localPosition + chunk.chunkData.positionForWorld + new Vector3(0.5f, 0.5f, 0.5f);
                    Vector3 dropDirection = Vector3.zero;
                    BlockDirectionEnum blockDirection = chunk.chunkData.GetBlockDirection(localPosition);
                    switch (blockDirection)
                    {
                        case BlockDirectionEnum.UpForward:
                            dropDirection = Vector3.back;
                            worldPositionForOut += new Vector3(0f, 1f, -1.5f);
                            break;
                        case BlockDirectionEnum.UpBack:
                            dropDirection = Vector3.forward;
                            worldPositionForOut += new Vector3(0f, 1f, 1.5f);
                            break;
                        case BlockDirectionEnum.UpLeft:
                            dropDirection = Vector3.left;
                            worldPositionForOut += new Vector3(-1.5f, 1f, 0f);
                            break;
                        case BlockDirectionEnum.UpRight:
                            dropDirection = Vector3.right;
                            worldPositionForOut += new Vector3(1.5f, 1f, 0);
                            break;
                    }
                    int itemAfterId = fireItemsId[0];
                    int itemAfterNum = fireItemsNum[0];

                    ItemsBean itemAfter = new ItemsBean();
                    itemAfter.itemId = itemAfterId;
                    itemAfter.number = itemAfterNum;

                    ItemDropBean itemDropData = new ItemDropBean(itemAfter, ItemDropStateEnum.DropPick, worldPositionForOut, dropDirection);
                    ItemsHandler.Instance.CreateItemCptDrop(itemDropData, null);
                }
            }
        }
        //保存数据
        if (isSaveData)
        {
            blockData.SetBlockMeta(blockMetaData);
            chunk.SetBlockData(blockData);
        }
    }

    #region 接口
    /// <summary>
    /// 道具放入
    /// </summary>
    public virtual void ItemsPut(Chunk chunk, Vector3Int localPosition, ItemsBean putItem)
    {
        if (putItem == null || putItem.itemId == 0 || putItem.number == 0)
        {
            return;
        }
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaInfernalFurnace blockMetaData);
        if (blockMetaData.listItem == null)
        {
            blockMetaData.listItem = new List<long>();
        }
        for (int i = 0; i < putItem.number; i++)
        {
            blockMetaData.listItem.Add(putItem.itemId);
        }
        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);
    }

    /// <summary>
    /// 道具取出
    /// </summary>
    public virtual ItemsBean ItemsOut(Chunk chunk, Vector3Int localPosition)
    {
        return null;
    }
    #endregion
}