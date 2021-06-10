using UnityEditor;
using UnityEngine;

public class ChunkDataBean
{
    //所有的方块合集
    public ushort[] arrayBlockIds;
    //所有方块的方向集合
    public byte[] arrayBlockDirection;
    //世界坐标
    public Vector3Int positionForWorld;

    public int chunkWidth;
    public int chunkHeight;

    public ChunkDataBean(Vector3Int wPosition, int chunkWidth, int chunkHeight)
    {
        this.chunkWidth = chunkWidth;
        this.chunkHeight = chunkHeight;
        this.positionForWorld = wPosition;

        arrayBlockIds = new ushort[chunkWidth * chunkHeight * chunkWidth];
        arrayBlockDirection = new byte[chunkWidth * chunkHeight * chunkWidth];
    }


    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlockForLocal(int x, int y, int z, ushort blockId, byte direction)
    {
        int index = GetIndexByPosition(x, y, z);
        arrayBlockIds[index] = blockId;
        arrayBlockDirection[index] = direction;
    }
    public void SetBlockForLocal(int x, int y, int z, BlockTypeEnum blockType, DirectionEnum direction)
    {
        SetBlockForLocal(x, y, z, (ushort)blockType, (byte)direction);
    }
    public void SetBlockForLocal(Vector3Int blockPosition, ushort blockId, byte direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, blockId, direction);
    }
    public void SetBlockForLocal(Vector3Int blockPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, (ushort)blockType, (byte)direction);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, blockType, DirectionEnum.UP);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, BlockTypeEnum blockType, DirectionEnum direction)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, blockType, direction);
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public void GetBlockForLocal(int x, int y, int z, out BlockTypeEnum blockType, out DirectionEnum direction)
    {
        int index = GetIndexByPosition(x, y, z);
        blockType = (BlockTypeEnum)arrayBlockIds[index];
        direction = (DirectionEnum)arrayBlockDirection[index];
    }
    public void GetBlockForLocal(Vector3Int blockPosition, out BlockTypeEnum blockType, out DirectionEnum direction)
    {
        GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, out blockType, out direction);
    }


    /// <summary>
    /// 获取下标
    /// </summary>
    public int GetIndexByPosition(Vector3Int position)
    {
        return GetIndexByPosition(position.x, position.y, position.z);
    }
    public int GetIndexByPosition(int x, int y, int z)
    {
        return x * chunkWidth * chunkHeight + y * chunkWidth + z;
    }
}