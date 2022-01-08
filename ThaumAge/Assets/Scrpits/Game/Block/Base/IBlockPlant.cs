using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public interface IBlockPlant
{

}

public static class BlockPlantExtension
{

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <returns></returns>
    public static string ToMetaData<T>(this T self, int growPro, bool isStartGrow) where T : IBlockPlant
    {
        return ToMetaData(growPro, isStartGrow);
    }

    public static string ToMetaData(int growPro, bool isStartGrow)
    {
        return $"{growPro}:{isStartGrow}";
    }

    public static void FromMetaData<T>(this T self, string data, out int growPro, out bool isStartGrow) where T : IBlockPlant
    {
        FromMetaData(data, out growPro, out isStartGrow);
    }

    public static void FromMetaData(string data, out int growPro, out bool isStartGrow)
    {
        string[] dataList = data.SplitForArrayStr(':');
        growPro = int.Parse(dataList[0]);
        isStartGrow = bool.Parse(dataList[1]);
    }

    /// <summary>
    /// 获取种植收获
    /// </summary>
    public static List<ItemsBean> GetPlantHarvest(BlockBean blockData, BlockInfoBean blockInfo)
    {
        List<ItemsBean> listData = new List<ItemsBean>();
        if (blockData == null || blockData.meta.IsNull())
        {
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockInfo.GetBlockType());
            listData.Add(new ItemsBean(itemsInfo.id, 1));
            return listData;
        }
        FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);
        Vector2Int[] uvPosition = blockInfo.GetUVPosition();
        if (growPro >= uvPosition.Length - 1)
        {
            //已经成熟
            long[] harvetsIds = blockInfo.plant_harvest.SplitForArrayLong('|');
            for (int i = 0; i < harvetsIds.Length; i++)
            {
                listData.Add(new ItemsBean(harvetsIds[i], 1));
            }
        }
        else
        {
            //没有成熟
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockInfo.GetBlockType());
            listData.Add(new ItemsBean(itemsInfo.id, 1));
        }
        return listData;
    }

    /// <summary>
    /// 初始化植物定点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void InitPlantVert<T>(this T self, Vector3[] vertsAdd) where T : IBlockPlant
    {
        //往下偏移的位置
        float offsetY = -1f / 16f;
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            vertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
    }

    /// <summary>
    /// 初始化植物数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void InitPlantData<T>(this T self, Chunk chunk, Vector3Int localPosition) where T : IBlockPlant
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    /// <summary>
    /// 刷新植物
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void RefreshPlant<T>(this T self, Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo) where T : IBlockPlant
    {
        chunk.chunkData.GetBlockForLocal(localPosition, out Block block, out DirectionEnum direction);

        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
        {
            string meta = self.ToMetaData(0, false);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        //获取成长周期
        self.FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);
        //成长周期+1
        if (isStartGrow)
        {
            //是否开始生长
            growPro++;
        }
        else
        {
            isStartGrow = true;
        }

        //设置新数据
        string newMeta = self.ToMetaData(growPro, isStartGrow);
        blockData.meta = newMeta;
        chunk.SetBlockData(blockData);
        //刷新
        WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, block, block, direction);

        //判断是否已经是最大生长周期
        Vector2Int[] arrayUVData = blockInfo.GetUVPosition();
        if (growPro >= arrayUVData.Length - 1)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
        }
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public static Vector2[] GetUVsAddForPlant<T>(this T self, Chunk chunk, Vector3Int localPosition, BlockInfoBean blockInfo) where T : IBlockPlant
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        self.FromMetaData(blockData.meta, out int growPro, out bool isStartGrow);

        Vector2 uvStartPosition = GetUVStartPositionForPlant(self, blockInfo, Block.uvWidth, growPro);
        Vector2[] uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + Block.uvWidth),
            new Vector2(uvStartPosition.x + Block.uvWidth,uvStartPosition.y + Block.uvWidth),
            new Vector2(uvStartPosition.x + Block.uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + Block.uvWidth),
            new Vector2(uvStartPosition.x + Block.uvWidth,uvStartPosition.y + Block.uvWidth),
            new Vector2(uvStartPosition.x + Block.uvWidth,uvStartPosition.y)
        };
        return uvsAdd;
    }

    /// <summary>
    /// 获取生长UV
    /// </summary>
    public static Vector2 GetUVStartPositionForPlant<T>(this T self, BlockInfoBean blockInfo, float uvWidth, int growth)
    {
        Vector2Int[] arrayUVData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (growth >= arrayUVData.Length)
        {
            //如果生长周期大于UV长度 则取最后一个
            uvStartPosition = new Vector2(uvWidth * arrayUVData[arrayUVData.Length - 1].y, uvWidth * arrayUVData[arrayUVData.Length - 1].x);
        }
        else
        {
            //按生长周期取UV
            uvStartPosition = new Vector2(uvWidth * arrayUVData[growth].y, uvWidth * arrayUVData[growth].x);
        }
        return uvStartPosition;
    }
}