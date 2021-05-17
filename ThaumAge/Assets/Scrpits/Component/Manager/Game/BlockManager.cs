using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockManager : BaseManager, IBlockInfoView
{
    protected BlockInfoController controllerForBlock;

    //方块信息列表
    protected Dictionary<BlockTypeEnum, BlockInfoBean> dicBlockInfo = new Dictionary<BlockTypeEnum, BlockInfoBean>();
    //注册方块列表
    protected Dictionary<BlockTypeEnum, Block> dicBlockRegister = new Dictionary<BlockTypeEnum, Block>();

    public virtual void Awake()
    {
        controllerForBlock = new BlockInfoController(this, this);
        controllerForBlock.GetAllBlockInfoData(InitBlockInfo);
        RegisterBlock();
    }

    /// <summary>
    /// 获取注册的方块
    /// </summary>
    /// <param name="blockId"></param>
    /// <returns></returns>
    public Block GetRegisterBlock(int blockId)
    {
        return GetRegisterBlock((BlockTypeEnum)blockId);
    }
    public Block GetRegisterBlock(BlockTypeEnum blockType)
    {
        if (dicBlockRegister.TryGetValue(blockType, out Block value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// 注册所有方块
    /// </summary>
    public void RegisterBlock()
    {
        List<BlockTypeEnum> listBlockType = EnumUtil.GetEnumValue<BlockTypeEnum>();
        for (int i = 0; i < listBlockType.Count; i++)
        {
            BlockTypeEnum blockType = listBlockType[i];
            //获取方块数据
            BlockInfoBean blockInfo = GetBlockInfo(blockType);
            string blockTypeName = EnumUtil.GetEnumName(blockType);
            //通过反射获取类
            Block block = ReflexUtil.CreateInstance<Block>("Block" + blockTypeName);
            if (block == null)
            {
                //如果没有指定类 则根据形状使用基础方块类
                BlockShapeEnum blockShape = blockInfo.GetBlockShape();
                string blockShapeName = EnumUtil.GetEnumName(blockShape);
                block = ReflexUtil.CreateInstance<Block>("Block" + blockShapeName);
            }
            RegisterBlock(blockType, block);
        }
    }

    public void RegisterBlock(BlockTypeEnum blockType, Block block)
    {
        if (!dicBlockRegister.ContainsKey(blockType))
        {
            dicBlockRegister.Add(blockType, block);
        }
    }


    /// <summary>
    /// 初始化方块信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBlockInfo(List<BlockInfoBean> listData)
    {
        dicBlockInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BlockInfoBean itemInfo = listData[i];
            dicBlockInfo.Add(itemInfo.GetBlockType(), itemInfo);
        }
    }

    /// <summary>
    /// 获取方块信息
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public BlockInfoBean GetBlockInfo(BlockTypeEnum blockType)
    {
        if (dicBlockInfo.TryGetValue(blockType, out BlockInfoBean blockInfo))
        {
            return blockInfo;
        }
        return null;
    }

    /// <summary>
    /// 获取所有方块信息
    /// </summary>
    /// <returns></returns>
    public List<BlockInfoBean> GetAllBackInfo()
    {
        List<BlockInfoBean> listData = new List<BlockInfoBean>();
        foreach (var itemData in dicBlockInfo.Values)
        {
            listData.Add(itemData);
        }
        return listData;
    }

         
    public BlockInfoBean GetBlockInfo(long blockId)
    {
        return GetBlockInfo((BlockTypeEnum)blockId);
    }

    #region 方块数据回调
    public void GetBlockInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }


    public void GetBlockInfoFail(string failMsg, Action action)
    {
        LogUtil.Log("获取方块数据失败");
    }
    #endregion
}