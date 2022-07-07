﻿using System;
using UnityEditor;
using UnityEngine;

public class ChunkSectionData
{
    //基础高度
    public int yBase;
    //所有的方块合集
    protected int[] arrayBlock;
    //所有方块的方向集合
    protected byte[] arrayBlockDirection;
    //小区块大小
    public int sectionSize;

    //空气方块数量
    public int airBlockNumber;

    public ChunkSectionData(int sectionSize, int yBase)
    {
        this.sectionSize = sectionSize;
        this.yBase = yBase;

        airBlockNumber = sectionSize * sectionSize * sectionSize;
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {
        Array.Clear(arrayBlock,0, arrayBlock.Length);
        Array.Clear(arrayBlockDirection, 0, arrayBlockDirection.Length);

        airBlockNumber = sectionSize * sectionSize * sectionSize;
    }

    /// <summary>
    /// 是否渲染
    /// </summary>
    public bool IsRender()
    {
        if (airBlockNumber == sectionSize * sectionSize * sectionSize)
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

        if (arrayBlockDirection == null)
            arrayBlockDirection = new byte[sectionSize * sectionSize * sectionSize];

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

        if (arrayBlock == null)
            arrayBlock = new int[sectionSize * sectionSize * sectionSize];

        arrayBlock[GetSectionIndex(x, y, z)] = (int)block.blockType;
        if (block == null || block.blockType == BlockTypeEnum.None)
        {
            airBlockNumber++;
        }
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public void GetBlock(int x, int y, int z, out int block, out byte direction)
    {
        int blockIndex = GetSectionIndex(x, y, z);
        if (arrayBlock == null)
            block = 0;
        else
            block = arrayBlock[blockIndex];

        if (arrayBlockDirection==null)
            direction =(int)BlockDirectionEnum.UpForward;
        else 
            direction = arrayBlockDirection[blockIndex];
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public int GetBlock(int x, int y, int z)
    {
        if (arrayBlock == null)
        {
            return 0;
        }
        return arrayBlock[GetSectionIndex(x, y, z)];
    }

    /// <summary>
    /// 获取方块方向
    /// </summary>
    public int GetBlockDirection(int x, int y, int z)
    {
        if (arrayBlockDirection == null)
        {
            return (int)BlockDirectionEnum.UpForward;
        }
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