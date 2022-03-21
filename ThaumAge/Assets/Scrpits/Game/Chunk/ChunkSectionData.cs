using UnityEditor;
using UnityEngine;

public class ChunkSectionData
{
    //基础高度
    public int yBase;
    //所有的方块合集
    public int[] arrayBlock;
    //所有方块的方向集合
    public byte[] arrayBlockDirection;
    //小区块大小
    public int sectionSize;

    //空气方块数量
    public int airBlockNumber;
    //正方形方块数量
    public int cubeBlockNumber;

    public ChunkSectionData(int sectionSize, int yBase)
    {
        this.sectionSize = sectionSize;
        this.yBase = yBase;
        arrayBlock = new int[sectionSize * sectionSize * sectionSize];
        arrayBlockDirection = new byte[sectionSize * sectionSize * sectionSize];

        airBlockNumber = sectionSize * sectionSize * sectionSize;
        cubeBlockNumber = 0;
    }

    /// <summary>
    /// 是否渲染
    /// </summary>
    public bool IsRender()
    {
        if (airBlockNumber == sectionSize * sectionSize * sectionSize || cubeBlockNumber == sectionSize * sectionSize * sectionSize)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlock(int x, int y, int z, Block block, byte direction)
    {
        SetBlock(x, y, z, block);
        arrayBlockDirection[GetSectionIndex(x, y, z)] = direction;
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlock(int x, int y, int z, Block block)
    {
        Block oldBlock = BlockHandler.Instance.manager.GetRegisterBlock(GetBlock(x, y, z));
        if (oldBlock == null || oldBlock.blockType == BlockTypeEnum.None)
        {
            airBlockNumber--;
        }
        else if (oldBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Cube)
        {
            cubeBlockNumber--;
        }

        arrayBlock[GetSectionIndex(x, y, z)] = (int)block.blockType;
        if (block == null || block.blockType == BlockTypeEnum.None)
        {
            airBlockNumber++;
        }
        else if (block.blockInfo.GetBlockShape() == BlockShapeEnum.Cube)
        {
            cubeBlockNumber++;
        }
    }


    /// <summary>
    /// 获取方块
    /// </summary>
    public void GetBlock(int x, int y, int z, out int block, out byte direction)
    {
        int blockIndex = GetSectionIndex(x, y, z);
        block = arrayBlock[blockIndex];
        direction = arrayBlockDirection[blockIndex];
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public int GetBlock(int x, int y, int z)
    {
        return arrayBlock[GetSectionIndex(x, y, z)];
    }

    /// <summary>
    /// 获取方块方向
    /// </summary>
    public int GetBlockDirection(int x, int y, int z)
    {
        return arrayBlockDirection[GetSectionIndex( x,  y,  z)];
    }

    /// <summary>
    /// 获取下标
    /// </summary>
    /// <returns></returns>
    public int GetSectionIndex(int x, int y, int z)
    {
        return x * sectionSize * sectionSize + y * sectionSize + z;
    }
}