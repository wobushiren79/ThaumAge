using UnityEditor;
using UnityEngine;

public class ChunkData
{
    public Chunk chunkLeft;
    public Chunk chunkRight;
    public Chunk chunkForward;
    public Chunk chunkBack;

    public ChunkSectionData[] chunkSectionDatas;

    //世界坐标
    public Vector3Int positionForWorld;

    public int chunkWidth;
    public int chunkHeight;

    public ChunkData(Vector3Int wPosition, int chunkWidth, int chunkHeight)
    {
        this.chunkWidth = chunkWidth;
        this.chunkHeight = chunkHeight;
        this.positionForWorld = wPosition;

        int sectionSize = chunkWidth;
        chunkSectionDatas = new ChunkSectionData[chunkHeight / chunkWidth];

        for (int i = 0; i < chunkSectionDatas.Length; i++)
        {
            chunkSectionDatas[i] = new ChunkSectionData(sectionSize, sectionSize * i);
        }
    }

    /// <summary>
    /// 初始化四周方块
    /// </summary>
    public void InitRoundChunk()
    {
        if (chunkLeft == null)
        {
            chunkLeft = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(-chunkWidth, 0, 0));
        }
        if (chunkRight == null)
        {
            chunkRight = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(chunkWidth, 0, 0));
        }
        if (chunkForward == null)
        {
            chunkForward = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, -chunkWidth));
        }
        if (chunkBack == null)
        {
            chunkBack = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, chunkWidth));
        }
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlockForLocal(int x, int y, int z, Block block, byte direction)
    {
        int yIndex = y / chunkWidth;
        ChunkSectionData chunkSection = chunkSectionDatas[yIndex];
        chunkSection.SetBlock(x, y % chunkWidth, z, block, direction);
    }

    public void SetBlockForLocal(int x, int y, int z, Block block, BlockDirectionEnum direction)
    {
        SetBlockForLocal(x, y, z, block, (byte)direction);
    }
    public void SetBlockForLocal(Vector3Int blockPosition, Block block, byte direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, block, direction);
    }

    public void SetBlockForLocal(Vector3Int blockPosition, Block block, BlockDirectionEnum direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, block, (byte)direction);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, Block block)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, block, BlockDirectionEnum.UpForward);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, Block block, BlockDirectionEnum direction)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, block, direction);
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public void GetBlockForLocal(int x, int y, int z, out Block block, out BlockDirectionEnum direction)
    {
        int yIndex = y / chunkWidth;
        ChunkSectionData chunkSection = chunkSectionDatas[yIndex];
        chunkSection.GetBlock(x, y % chunkWidth, z, out int blockType, out byte blockDirection);
        block = BlockHandler.Instance.manager.GetRegisterBlock(blockType);
        direction = (BlockDirectionEnum)blockDirection;
    }

    public void GetBlockForLocal(Vector3Int blockPosition, out Block block, out BlockDirectionEnum direction)
    {
        GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, out block, out direction);
    }

    public Block GetBlockForLocal(int x, int y, int z)
    {
        int yIndex = y / chunkWidth;
        ChunkSectionData chunkSection = chunkSectionDatas[yIndex];
        int blockType = chunkSection.GetBlock(x, y % chunkWidth, z);
        return BlockHandler.Instance.manager.GetRegisterBlock(blockType);
    }

    public Block GetBlockForLocal(Vector3Int blockPosition)
    {
        return GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z);
    }

    public BlockDirectionEnum GetBlockDirection(int x, int y, int z)
    {
        int yIndex = y / chunkWidth;
        ChunkSectionData chunkSection = chunkSectionDatas[yIndex];
        return (BlockDirectionEnum)chunkSection.GetBlockDirection(x, y % chunkWidth, z);
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

    public Vector3Int GetPositionByIndex(int index)
    {
        return new Vector3Int((index % (chunkWidth * chunkWidth * chunkHeight)) / (chunkWidth * chunkHeight), (index % (chunkWidth * chunkHeight)) / chunkWidth, index % chunkWidth);
    }
}