using UnityEditor;
using UnityEngine;

public class BlockBaseLiquid : Block
{

    /// <summary>
    /// 刷新方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.RefreshBlock(chunk, localPosition, direction);
        //刷新的时候注册事件 
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;
        //设置周围方块
        SetCloseBlock(worldPosition);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 设置靠近的方块为水方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    public void SetCloseBlock(Vector3Int worldPosition)
    {
        Vector3Int downWorldPosition = worldPosition + Vector3Int.down;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(downWorldPosition, out Block downBlock, out DirectionEnum downBlockDirection, out Chunk downChunk);
        if (downBlock == null)
            return;
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None)
        {
            //如果是空方块 替换成当前方块
            downChunk.SetBlockForWorld(downWorldPosition, blockType);
            //更新区块
            WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
            return;
        }
    }


    /// <summary>
    /// 删除方块
    /// </summary>
    /// <param name="chunk"></param>
    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.DestoryBlock(chunk, localPosition);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }
}