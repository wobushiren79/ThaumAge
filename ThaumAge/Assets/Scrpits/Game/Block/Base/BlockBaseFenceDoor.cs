using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class BlockBaseFenceDoor : Block
{

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

        BlockMetaDoor blockDoorData = FromMetaData<BlockMetaDoor>(blockData.meta);
        if (blockDoorData == null)
        {
            blockDoorData = new BlockMetaDoor();
            blockDoorData.state = 0;
            blockDoorData.linkBasePosition = new Vector3IntBean(worldPosition);
        }

        Vector3Int baseWorldPosition = blockDoorData.GetBasePosition();
        GameObject objDoor = BlockHandler.Instance.GetBlockObj(baseWorldPosition);
        Transform tfDoorL = objDoor.transform.Find("Door_L");
        Transform tfDoorR = objDoor.transform.Find("Door_R");

        if (blockDoorData.state == 0)
        {
            tfDoorR.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);
            tfDoorL.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
            blockDoorData.state = 1;
        }
        else if (blockDoorData.state == 1 || blockDoorData.state == 2)
        {
            //如果是开门状态 则关门
            tfDoorR.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            tfDoorL.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            blockDoorData.state = 0;
        }
        blockData.meta = ToMetaData(blockDoorData);
    }
}