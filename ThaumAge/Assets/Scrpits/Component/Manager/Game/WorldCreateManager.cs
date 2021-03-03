using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //存储着世界中所有的Chunk
    public List<TerrainForChunk> chunks = new List<TerrainForChunk>();

    public  TerrainForChunk GetChunk(Vector3 wPos)
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 tempPos = chunks[i].transform.position;

            //wPos是否超出了Chunk的XZ平面的范围
            if ((wPos.x < tempPos.x) || (wPos.z < tempPos.z) || (wPos.x >= tempPos.x + 20) || (wPos.z >= tempPos.z + 20))
                continue;

            return chunks[i];
        }
        return null;
    }

    /// <summary>
    /// 增加区域
    /// </summary>
    /// <param name="chunk"></param>
    public void AddChunk(TerrainForChunk chunk)
    {
        chunks.Add(chunk);
    }

}