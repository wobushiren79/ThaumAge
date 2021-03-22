using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockHandler : BaseHandler<BlockHandler, BlockManager>
{
    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public Block CreateBlock(Chunk chunk, Vector3Int position, BlockTypeEnum blockType)
    {
        string blockTypeName = EnumUtil.GetEnumName(blockType);
        //设置数据
        BlockBean blockData = new BlockBean(blockType, position);
        //获取方块数据
        BlockInfoBean blockInfo = manager.GetBlockInfo(blockType);
        //通过反射获取类
        Block block = ReflexUtil.CreateInstance<Block>("Block" + blockTypeName);
        if (block == null)
        {
            //如果没有指定类 则根据形状使用基础方块类
            BlockShapeEnum blockShape= blockInfo.GetBlockShape();
            string blockShapeName = EnumUtil.GetEnumName(blockShape);
            block = ReflexUtil.CreateInstance<Block>("Block"+ blockShapeName);
        }
        block.SetData(chunk, position, blockData);
        return block;
    }



}