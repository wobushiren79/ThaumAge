using UnityEditor;
using UnityEngine;

public interface IBlockPlant
{

}

public static class BlockPlantExtension
{

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <returns></returns>
    public static string ToMetaData<T>(this T self, int growth,bool isStartGrow) where T : IBlockPlant
    {
        return $"{growth}:{isStartGrow}";
    }

    public static void FromMetaData<T>(this T self, string data, out int growth,out bool isStartGrow) where T : IBlockPlant
    {
        string[] dataList= StringUtil.SplitBySubstringForArrayStr(data,':');
        growth = int.Parse(dataList[0]);
        isStartGrow = bool.Parse(dataList[1]);
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
            return;
        //获取成长周期
        self.FromMetaData(blockData.meta, out int growth,out bool isStartGrow);
        //成长周期+1
        if (isStartGrow)
        {
            //是否开始生长
            growth++;
        }
        else
        {
            isStartGrow = true;
        }

        //设置新数据
        string newMeta =  self.ToMetaData(growth, isStartGrow);
        chunk.SetBlockForLocal(localPosition, blockInfo.GetBlockType(), direction, newMeta);
        WorldCreateHandler.Instance.HandleForUpdateChunk(chunk, localPosition, block, block, direction);
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public static Vector2[] GetUVsAddForPlant<T>(this T self, BlockInfoBean blockInfo, float uvWidth, int growth) where T : IBlockPlant
    {
        Vector2 uvStartPosition = GetUVStartPositionForPlant(self, blockInfo, uvWidth, growth);
        Vector2[] uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
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