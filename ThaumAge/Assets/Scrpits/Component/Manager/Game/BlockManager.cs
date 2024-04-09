using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockManager : BaseManager
{
    //方块信息列表
    protected BlockInfoBean[] arrayBlockInfo = new BlockInfoBean[EnumExtension.GetEnumMaxIndex<BlockTypeEnum>() + 1];
    //注册方块列表
    protected Block[] arrayBlockRegister = new Block[EnumExtension.GetEnumMaxIndex<BlockTypeEnum>() + 1];
    //方块模型列表
    protected GameObject[] arrayBlockModel = new GameObject[EnumExtension.GetEnumMaxIndex<BlockTypeEnum>() + 1];
    //存储着所有的材质
    public Material[] arrayBlockMat;


    //路径-破碎方块
    public static string pathForBlockCptBreak = "Assets/Prefabs/Game/BlockBreak.prefab";
    //方块破碎模型
    private GameObject _blockBreakModel;
    public GameObject blockBreakModel
    {
        get
        {
            if (_blockBreakModel == null)
            {
                _blockBreakModel = LoadAddressablesUtil.LoadAssetSync<GameObject>(pathForBlockCptBreak);
            }
            return _blockBreakModel;
        }
    }


    //路径-傀儡核心选择方块
    public static string pathForBlockCptGolemCore = "Assets/Prefabs/Game/Block/BlockSelectGolemCore.prefab";
    //傀儡核心选择方块
    private GameObject _blockGolemCoreSelectModel;
    public GameObject blockGolemCoreSelectModel 
    {
        get
        {
            if (_blockGolemCoreSelectModel == null)
            {
                _blockGolemCoreSelectModel = LoadAddressablesUtil.LoadAssetSync<GameObject>(pathForBlockCptGolemCore);
            }
            return _blockGolemCoreSelectModel;
        }
    }

    //路径-方块模型
    public static string pathForBlockModel = "Assets/Prefabs/Block";

    //路径-方块材质 （使用标签）
    public static string pathForBlockMats = "BlockMats";

    public virtual void Awake()
    {
        InitData();
    }

    public void InitData()
    {
        InitBlockInfo();
        RegisterBlock();
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    public void LoadResources(Action callBack)
    {
        if (!arrayBlockMat.IsNull())
        {
            callBack?.Invoke();
            return;
        }
        //加载所有方块材质球
        LoadAddressablesUtil.LoadAssetsAsync<Material>(pathForBlockMats, (data) =>
        {
            IList<Material> listMat = data.Result;

            int maxMatIndex = 0;
            for (int i = 0; i < listMat.Count; i++)
            {
                //按照名字中的下标 确认每个材质球的顺序
                Material itemMat = listMat[i];
                string[] nameList = itemMat.name.SplitForArrayStr('_');
                int indexMat = int.Parse(nameList[1]);
                if(indexMat > maxMatIndex)
                    maxMatIndex = indexMat;
            }
            arrayBlockMat = new Material[maxMatIndex+1];
            for (int i = 0; i < listMat.Count; i++)
            {
                //按照名字中的下标 确认每个材质球的顺序
                Material itemMat = listMat[i];
                string[] nameList = itemMat.name.SplitForArrayStr('_');
                int indexMat = int.Parse(nameList[1]);
                arrayBlockMat[indexMat] = itemMat;
            }
            callBack?.Invoke();
        });
    }

    /// <summary>
    /// 获取所有材质
    /// </summary>
    /// <returns></returns>
    public Material[] GetAllBlockMaterial()
    {
        return arrayBlockMat;
    }

    public Material GetBlockMaterial(BlockMaterialEnum blockMaterial)
    {
        return arrayBlockMat[(int)blockMaterial];
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
            objModel = LoadAddressablesUtil.LoadAssetSync<GameObject>($"{pathForBlockModel}/{modelName}.prefab");
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
    /// 获取注册的方块形状
    /// </summary>
    /// <param name="blockShapeEnum"></param>
    /// <returns></returns>
    public BlockShape GetRegisterBlockShape(Block block, BlockShapeEnum blockShapeEnum)
    {
        string blockShapeName = blockShapeEnum.GetEnumName();
        //通过反射获取类
        string className = $"BlockShape{blockShapeName}";
        BlockShape blockShape = ReflexUtil.CreateInstance<BlockShape>(className, new object[] { block });

        if (blockShape == null)
            blockShape = new BlockShape(block);
        return blockShape;
    }

    /// <summary>
    /// 注册所有方块
    /// </summary>
    public void RegisterBlock()
    {
        List<BlockTypeEnum> listBlockType = EnumExtension.GetEnumValue<BlockTypeEnum>();
        for (int i = 0; i < listBlockType.Count; i++)
        {
            BlockTypeEnum blockType = listBlockType[i];
            BlockInfoBean blockInfo = GetBlockInfo(blockType);
            string blockTypeName;
            if (!blockInfo.link_class.IsNull())
            {
                blockTypeName = blockInfo.link_class;
            }
            else
            {
                blockTypeName = $"BlockType{EnumExtension.GetEnumName(blockType)}";
            }

            //通过反射获取类
            Block block = ReflexUtil.CreateInstance<Block>(blockTypeName);
            if (block == null) block = new Block();
            block.SetData(blockType);
            arrayBlockRegister[(int)blockType] = block;
        }
    }
    /// <summary>
    /// 初始化方块信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBlockInfo()
    {
        var dicAllData = BlockInfoCfg.GetAllData();
        foreach (var itemData in dicAllData)
        {
            arrayBlockInfo[itemData.Key] = itemData.Value;
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
    public BlockInfoBean[] GetAllBlockInfo()
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