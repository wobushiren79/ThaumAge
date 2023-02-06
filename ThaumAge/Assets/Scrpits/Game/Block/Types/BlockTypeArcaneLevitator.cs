using UnityEditor;
using UnityEngine;

public class BlockTypeArcaneLevitator : Block
{

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int refreshType = 7, int updateChunkType = 1)
    {
        base.RefreshBlock(chunk, localPosition, direction, refreshType, updateChunkType);
        //先刷新自己
        RefreshObjModel(chunk, localPosition, refreshType);
        //刷新上方方块
        if (refreshType == (int)DirectionEnum.Down)
        {
            RefreshUpDownBlock(chunk, localPosition, 1);
        }
        //刷新下方方块
        else if (refreshType == (int)DirectionEnum.UP)
        {
            RefreshUpDownBlock(chunk, localPosition, -1);
        }
    }

    public override void RefreshObjModel(Chunk chunk, Vector3Int localPosition, int refreshType)
    {
        //检测上方有多少个同样的方块
        int upBlockNum = CheckUpBlock(chunk, localPosition);

        GameObject objArcaneLevitator = chunk.GetBlockObjForLocal(localPosition);
        if (objArcaneLevitator == null)
            return;
        BoxCollider boxCollider = objArcaneLevitator.GetComponent<BoxCollider>();
        boxCollider.center = new Vector3(0, 3 + upBlockNum * 5 + upBlockNum, 0);
    }

    /// <summary>
    /// 检测上方有几个一样的悬浮方块
    /// </summary>
    /// <returns></returns>
    public virtual int CheckUpBlock(Chunk chunk, Vector3Int localPosition)
    {
        int num = 0;
        if (chunk == null)
            return num;
        while (num <= chunk.chunkData.chunkHeight)
        {
            chunk.GetBlockForLocal(localPosition + new Vector3Int(0, num + 1, 0), out Block itemBlock, out BlockDirectionEnum itemBlockDirection, out Chunk itemChunk);
            if (itemChunk == null || itemBlock == null)
            {
                break;
            }
            if (itemBlock.blockType != BlockTypeEnum.ArcaneLevitator)
            {
                break;
            }
            num++;
        }
        return num;
    }

    /// <summary>
    /// 刷新上下的方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="updown"></param>
    public virtual void RefreshUpDownBlock(Chunk chunk, Vector3Int localPosition, int updown)
    {
        if (chunk == null)
            return;
        Vector3Int itemLocalPosition = localPosition + new Vector3Int(0, updown, 0);
        chunk.GetBlockForLocal(itemLocalPosition, out Block itemBlock, out BlockDirectionEnum itemBlockDirection, out Chunk itemChunk);
        if (itemChunk == null || itemBlock == null)
        {
            return;
        }
        if (itemBlock.blockType != BlockTypeEnum.ArcaneLevitator)
        {
            return;
        }
        if (updown == 1)
        {
            itemBlock.RefreshBlock(itemChunk, itemLocalPosition, itemBlockDirection, (int)DirectionEnum.Down);
        }
        else if (updown == -1)
        {
            itemBlock.RefreshBlock(itemChunk, itemLocalPosition, itemBlockDirection, (int)DirectionEnum.UP);
        }
    }
}