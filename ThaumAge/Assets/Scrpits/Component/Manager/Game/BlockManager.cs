using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockManager : BaseManager, IBlockInfoView
{
   
    protected BlockInfoController controllerForBlock;

    //方块信息列表
    protected BlockInfoBean[] arrayBlockInfo = new BlockInfoBean[EnumUtil.GetEnumMaxIndex<BlockTypeEnum>() + 1];
    //注册方块列表
    protected Block[] arrayBlockRegister = new Block[EnumUtil.GetEnumMaxIndex<BlockTypeEnum>() + 1];
    //方块模型列表
    protected GameObject[] arrayBlockModel = new GameObject[EnumUtil.GetEnumMaxIndex<BlockTypeEnum>() + 1];

    public virtual void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        controllerForBlock = new BlockInfoController(this, this);
        controllerForBlock.GetAllBlockInfoData(InitBlockInfo);
        RegisterBlock();
    }

    /// <summary>
    /// 获取方块的模型
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public GameObject GetBlockModel(ushort blockId, string modelName)
    {
        GameObject objModel = arrayBlockModel[blockId];
        if (objModel == null)
        {
            objModel = GetModel<GameObject>("block/block", modelName);
            arrayBlockModel[blockId] = objModel;
        }
        return objModel;
    }

    /// <summary>
    /// 获取注册的方块
    /// </summary>
    /// <param name="blockId"></param>
    /// <returns></returns>
    public Block GetRegisterBlock(int blockId)
    {
        return arrayBlockRegister[blockId];
    }

    public Block GetRegisterBlock(BlockTypeEnum blockType)
    {
        return GetRegisterBlock((int)blockType);
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
            block.blockInfo = blockInfo;
            block.blockType = blockType;
            RegisterBlock(blockType, block);
        }
    }

    public void RegisterBlock(BlockTypeEnum blockType, Block block)
    {
        arrayBlockRegister[(int)blockType] = block;
    }

    /// <summary>
    /// 初始化方块信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBlockInfo(List<BlockInfoBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            BlockInfoBean itemInfo = listData[i];
            arrayBlockInfo[itemInfo.id] = itemInfo;
        }
    }

    /// <summary>
    /// 获取方块信息
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public BlockInfoBean GetBlockInfo(BlockTypeEnum blockType)
    {
        return GetBlockInfo((int)blockType);
    }
    public BlockInfoBean GetBlockInfo(int blockId)
    {
        return arrayBlockInfo[blockId];
    }

    /// <summary>
    /// 获取除空气外的所有方块信息
    /// </summary>
    /// <returns></returns>
    public BlockInfoBean[] GetAllBackInfo()
    {
        return arrayBlockInfo;
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