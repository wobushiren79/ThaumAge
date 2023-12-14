using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Animations.AimConstraint;

public class ChunkData
{
    public Chunk chunkSelf;
    public Chunk chunkLeft;
    public Chunk chunkRight;
    public Chunk chunkForward;
    public Chunk chunkBack;

    public ChunkSectionData[] chunkSectionDatas;

    //世界坐标
    public Vector3Int positionForWorld;

    public int chunkWidth;
    public int chunkHeight;

    //生态类型
    public BiomeTypeEnum biomeType;
    public BiomeTypeEnum biomeTypeL;
    public BiomeTypeEnum biomeTypeR;
    public BiomeTypeEnum biomeTypeF;
    public BiomeTypeEnum biomeTypeB;
    public ChunkData(Chunk chunkSelf, Vector3Int wPosition, int chunkWidth, int chunkHeight)
    {
        this.chunkSelf = chunkSelf;
        this.chunkWidth = chunkWidth;
        this.chunkHeight = chunkHeight;
        this.positionForWorld = wPosition;

        int sectionSize = chunkWidth;
        chunkSectionDatas = new ChunkSectionData[chunkHeight / chunkWidth];

        for (int i = 0; i < chunkSectionDatas.Length; i++)
        {
            chunkSectionDatas[i] = new ChunkSectionData(sectionSize, sectionSize * i);
        }
        //设置生态
        WorldTypeEnum worldType = WorldCreateHandler.Instance.manager.worldType;
        int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        biomeType = BiomeHandler.Instance.manager.GetBiomeType(wPosition, chunkWidth, worldType, seed);

        biomeTypeL = BiomeHandler.Instance.manager.GetBiomeType(wPosition + new Vector3Int(-chunkWidth, 0, 0), chunkWidth, worldType, seed);
        biomeTypeR = BiomeHandler.Instance.manager.GetBiomeType(wPosition + new Vector3Int(chunkWidth, 0, 0), chunkWidth, worldType, seed);
        biomeTypeF = BiomeHandler.Instance.manager.GetBiomeType(wPosition + new Vector3Int(0, 0, -chunkWidth), chunkWidth, worldType, seed);
        biomeTypeB = BiomeHandler.Instance.manager.GetBiomeType(wPosition + new Vector3Int(0, 0, chunkWidth), chunkWidth, worldType, seed);
    }

    /// <summary>
    /// 初始化四周方块
    /// </summary>
    public void InitRoundChunk()
    {
        if (chunkLeft == null)
        {
            chunkLeft = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(-chunkWidth, 0, 0));
            if (chunkLeft != null && chunkLeft.chunkData != null)
            {
                chunkLeft.chunkData.chunkRight = chunkSelf;
            }
        }
        if (chunkRight == null)
        {
            chunkRight = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(chunkWidth, 0, 0));
            if (chunkRight != null && chunkRight.chunkData != null)
            {
                chunkRight.chunkData.chunkLeft = chunkSelf;
            }
        }
        if (chunkForward == null)
        {
            chunkForward = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, -chunkWidth));
            if (chunkForward != null && chunkForward.chunkData != null)
            {
                chunkForward.chunkData.chunkBack = chunkSelf;
            }
        }
        if (chunkBack == null)
        {
            chunkBack = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, chunkWidth));
            if (chunkBack != null && chunkBack.chunkData != null)
            {
                chunkBack.chunkData.chunkForward = chunkSelf;
            }
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

    public int GetBlockForLocalBase(int x, int y, int z)
    {
        int yIndex = y / chunkWidth;
        ChunkSectionData chunkSection = chunkSectionDatas[yIndex];
        return chunkSection.GetBlock(x, y % chunkWidth, z);
    }

    public void GetBlockForLocal(Vector3Int blockPosition, out Block block, out BlockDirectionEnum direction)
    {
        GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, out block, out direction);
    }

    public Block GetBlockForLocal(int x, int y, int z)
    {
        int blockType = GetBlockForLocalBase(x, y, z);
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

    public BlockDirectionEnum GetBlockDirection(Vector3Int blockPosition)
    {
        return GetBlockDirection(blockPosition.x, blockPosition.y, blockPosition.z);
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