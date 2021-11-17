using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockPloughGrass : BlockCubeCuboid
{

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, DirectionEnum buildDirection)
    {
        Vector2 uvStartPosition = GetUVStartPosition(buildDirection);

        List<Vector2> uvs = chunkMeshData.uvs;
        WorldDataBean worldData = chunk.GetWorldData();
        if (worldData.chunkData.GetBlockData(localPosition.x, localPosition.y, localPosition.z, out BlockBean blockData))
        {
            FromMetaData(blockData.meta, out int rotate);
            if (rotate == 1)
            {
                uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
                uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
                uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));
                uvs.Add(uvStartPosition);
                return;
            }
        }
        uvs.Add(uvStartPosition);
        uvs.Add(new Vector2(uvStartPosition.x, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y + uvWidth));
        uvs.Add(new Vector2(uvStartPosition.x + uvWidth, uvStartPosition.y));
    }

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <param name="rotate">0横 1竖</param>
    /// <returns></returns>
    public static string ToMetaData(int rotate)
    {
        return $"{rotate}";
    }

    public static void FromMetaData(string data, out int rotate)
    {
        rotate = int.Parse(data);
    }
}