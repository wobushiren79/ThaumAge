using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockPloughGrass : BlockShapeCubeCuboid
{
    protected Vector2[] uvsAddUpRotate;
    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        Vector2 uvStart = GetUVStartPosition(DirectionEnum.UP);
        uvsAddUpRotate = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),
            new Vector2(uvStart.x,uvStart.y),
        };
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum face, Vector2[] uvsAdd)
    {
        WorldDataBean worldData = chunk.GetWorldData();
        if (face == DirectionEnum.UP && worldData.chunkData.GetBlockData(localPosition.x, localPosition.y, localPosition.z, out BlockBean blockData))
        {
            FromMetaData(blockData.meta, out int rotate);
            if (rotate == 1)
            {
                AddUVs(chunk.chunkMeshData.uvs, uvsAddUpRotate);
                return;
            }
        }
        base.BaseAddUVs(chunk, localPosition, direction, face, uvsAdd);
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