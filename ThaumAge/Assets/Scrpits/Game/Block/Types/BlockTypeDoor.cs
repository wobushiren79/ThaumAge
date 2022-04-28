using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class BlockTypeDoor : Block
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        //如果是放置
        if (state == 1)
        {

        }
    }

    public override string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        BlockDoorBean blockDoor = new BlockDoorBean();
        blockDoor.level = 0;
        blockDoor.linkBasePosition = new Vector3IntBean(worldPosition);
        return ToMetaData(blockDoor);
    }

    /// <summary>
    /// 道具放置
    /// </summary>
    public override void ItemUse(
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk, 
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk, 
        BlockDirectionEnum direction, string metaData)
    {
        base.ItemUse(targetWorldPosition, targetBlockDirection, targetBlock, targetChunk, closeWorldPosition, closeBlockDirection, closeBlock, closeChunk, direction, metaData);

        CreateLinkBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, new List<Vector3Int>() { Vector3Int.up });
    }



    public override void CreateBlockModel(Chunk chunk, Vector3Int localPosition)
    {
        //如果有模型。则创建模型
        if (!blockInfo.model_name.IsNull())
        {
            //获取数据
            BlockBean blockData = chunk.GetBlockData(localPosition);
            if (blockData != null)
            {
                BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
                if (blockDoorData != null)
                {
                    if (blockDoorData.level == 1)
                        return;
                }
            }
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        DestoryLinkBlock(chunk, localPosition, direction, new List<Vector3Int>() { Vector3Int.up });
    }

    /// <summary>
    /// 互动
    /// </summary>
    /// <param name="worldPosition"></param>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out BlockDirectionEnum direction, out Chunk chunk);
        //获取数据
        BlockBean blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);

        BlockDoorBean blockDoorData = GetLinkBaseBlockData<BlockDoorBean>(blockData.meta);
        if (blockDoorData == null)
        {
            blockDoorData = new BlockDoorBean();
            blockDoorData.state = 0;
            blockDoorData.linkBasePosition = new Vector3IntBean(worldPosition);
        }

        Vector3Int baseWorldPosition = blockDoorData.GetBasePosition();
        GameObject objDoor = BlockHandler.Instance.GetBlockObj(baseWorldPosition);
        Transform tfDoor = objDoor.transform.Find("Door");
        if (blockDoorData.state == 0)
        {
            int directionFace = (int)direction % 10;
            if (user == null)
            {
                tfDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
                blockDoorData.state = 1;
            }
            else
            {
                //如果是在X轴上
                switch (directionFace)
                {
                    case 1:
                        if (user.transform.position.x > worldPosition.x)
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
                            blockDoorData.state = 1;
                        }
                        else
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
                            blockDoorData.state = 2;
                        }
                        break;
                    case 2:
                        if (user.transform.position.x > worldPosition.x)
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
                            blockDoorData.state = 2;
                        }
                        else
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
                            blockDoorData.state = 1;
                        }
                        break;
                    case 3:
                        if (user.transform.position.z > worldPosition.z)
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
                            blockDoorData.state = 2;
                        }
                        else
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
                            blockDoorData.state = 1;
                        }
                        break;
                    case 4:
                        if (user.transform.position.z > worldPosition.z)
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
                            blockDoorData.state = 1;
                        }
                        else
                        {
                            tfDoor.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
                            blockDoorData.state = 2;
                        }
                        break;
                }
            }
        }
        else if (blockDoorData.state == 1 || blockDoorData.state == 2)
        {
            //如果是开门状态 则关门
            tfDoor.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            blockDoorData.state = 0;
        }
        SaveLinkBaseBlockData(baseWorldPosition, blockDoorData);
    }

}