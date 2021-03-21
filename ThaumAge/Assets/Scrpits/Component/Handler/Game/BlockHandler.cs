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
        //通过反射获取类
        Block block = ReflexUtil.CreateInstance<Block>("Block" + blockTypeName);
        if (block == null)
        {
             block = ReflexUtil.CreateInstance<Block>("BlockCube");
        }
        block.SetData(chunk, position, blockData);
        return block;
    }



}