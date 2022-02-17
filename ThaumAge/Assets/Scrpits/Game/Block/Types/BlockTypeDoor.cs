using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class BlockTypeDoor : Block
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int state)
    {
        base.InitBlock(chunk, localPosition, blockDirection, state);
        //如果是放置
        if (state == 1)
        {
            CreateLinkBlock(chunk, localPosition, blockDirection, new List<Vector3Int>() { Vector3Int.up });
        }
    }

    public override void CreateBlockModel(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection)
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

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        BlockShapeCustom blockShapeCustom = blockShape as BlockShapeCustom;
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockDoorBean blockDoorData = null;
        if (blockData != null)
        {
            blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
        }
        if (blockDoorData == null)
        {
            blockDoorData = new BlockDoorBean();
            blockDoorData.state = 0;
            blockDoorData.linkBasePosition = new Vector3IntBean(localPosition + chunk.chunkData.positionForWorld);
        }
        blockShapeCustom.BuildBlock(chunk, localPosition, direction);
    }

    /// <summary>
    /// 互动
    /// </summary>
    /// <param name="worldPosition"></param>
    public override void Interactive(GameObject user, Vector3Int worldPosition)
    {
        base.Interactive(user, worldPosition);
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out BlockDirectionEnum direction, out Chunk chunk);
        //获取数据
        BlockBean blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
        Vector3Int baseWorldPosition = worldPosition;

        BlockDoorBean blockDoorData = null;
        if (blockData != null)
        {
            blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
            if (blockDoorData != null)
            {
                baseWorldPosition = blockDoorData.linkBasePosition.GetVector3Int();
            }
        }
        if (blockDoorData == null)
        {
            blockDoorData = new BlockDoorBean();
            blockDoorData.state = 0;
            blockDoorData.linkBasePosition = new Vector3IntBean(worldPosition);
        }

        GameObject objDoor = BlockHandler.Instance.GetBlockObj(baseWorldPosition);
        Transform tfDoor = objDoor.transform.Find("Door");
        if (blockDoorData.state == 0)
        {               
            int directionFace = (int)direction % 10;
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
        else if (blockDoorData.state == 1 || blockDoorData.state == 2)
        {
            //如果是开门状态 则关门
            tfDoor.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            blockDoorData.state = 0;
        }
        List<Vector3Int> listLinkPosition = new List<Vector3Int>() { Vector3Int.zero, Vector3Int.up };
        SaveLinkBlockData(baseWorldPosition, listLinkPosition, blockDoorData);
    }

}