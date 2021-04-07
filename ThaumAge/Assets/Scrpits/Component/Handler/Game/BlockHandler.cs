using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
        //设置数据
        BlockBean blockData = new BlockBean(blockType, position, position + chunk.worldPosition);
        return CreateBlock(chunk, blockData);
    }

    /// <summary>
    /// 创建方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    public Block CreateBlock(Chunk chunk, BlockBean blockData)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        //获取方块数据
        //BlockInfoBean blockInfo = manager.GetBlockInfo(blockType);

        //string blockTypeName = EnumUtil.GetEnumName(blockType);
        ////通过反射获取类
        //Block block = ReflexUtil.CreateInstance<Block>("Block" + blockTypeName);
        //if (block == null)
        //{
        //    //如果没有指定类 则根据形状使用基础方块类
        //    BlockShapeEnum blockShape = blockInfo.GetBlockShape();
        //    string blockShapeName = EnumUtil.GetEnumName(blockShape);
        //    block = ReflexUtil.CreateInstance<Block>("Block" + blockShapeName);
        //}

        //Block block;
        //BlockShapeEnum blockShape = blockInfo.GetBlockShape();
        //switch (blockShape)
        //{
        //    case BlockShapeEnum.Cross:
        //        block = new BlockCross();
        //        break;
        //    case BlockShapeEnum.CrossOblique:
        //        block = new BlockCrossOblique();
        //        break;
        //    case BlockShapeEnum.Cube:
        //        block = new BlockCube();
        //        break;
        //    case BlockShapeEnum.CubeCuboid:
        //        block = new BlockCubeCuboid();
        //        break;
        //    case BlockShapeEnum.CubeTransparent:
        //        block = new BlockCubeTransparent();
        //        break;
        //    case BlockShapeEnum.Custom:
        //        block = new BlockCustom();
        //        break;
        //    case BlockShapeEnum.None:
        //        block = new BlockNone();
        //        break;
        //    default:
        //        block = new BlockCube();
        //        break;
        //}
        //block.SetData(chunk, blockData.localPosition.GetVector3Int(), blockData);

        Type type = manager.GetRegisterBlock(blockType).GetType();

        //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
        //Block block = asm.CreateInstance(type.FullName) as Block;

        //ConstructorInfo conStruct = type.GetConstructor(new Type[] {  });
        //Block block = conStruct.Invoke(new object[] { }) as Block;

        Block block = Activator.CreateInstance(type) as Block;
        block.SetData(chunk, blockData.localPosition.GetVector3Int(), blockData);
        return block;
    }

}