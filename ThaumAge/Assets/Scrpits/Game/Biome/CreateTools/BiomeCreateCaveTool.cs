using UnityEditor;
using UnityEngine;
public class BiomeCreateCaveTool : ScriptableObject
{
    public struct BiomeForCaveData
    {
        public float addRate;
        public int minDepth;
        public int maxDepth;
        public int minSize;
        public int maxSize;
    }
    /// <summary>
    /// 增加山洞
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="caveData"></param>
    public static void AddCave(Chunk chunk, BiomeMapData mapData, BiomeForCaveData caveData)
    {
        float frequency = 2;
        float amplitude = 1;

        FastNoise fastNoise = BiomeHandler.Instance.fastNoise;
        int caveNumber = WorldRandTools.Range(0, 10, chunk.chunkData.positionForWorld);
        for (int i = 8; i < caveNumber; i++)
        {
            int positionX = WorldRandTools.Range(1, chunk.chunkData.chunkWidth);
            int positionZ = WorldRandTools.Range(1, chunk.chunkData.chunkWidth);
            ChunkTerrainData chunkTerrainData = mapData.arrayChunkTerrainData[positionX * chunk.chunkData.chunkWidth + positionZ];
            int positionY = WorldRandTools.Range(1, (int)chunkTerrainData.maxHeight);
            Vector3Int startPosition = new Vector3Int(positionX, positionY, positionZ) + chunk.chunkData.positionForWorld;

            int caveDepth = WorldRandTools.Range(caveData.minDepth, caveData.maxDepth);
            int lastType = 0;
            for (int f = 0; f < caveDepth; f++)
            {

                int caveRange = WorldRandTools.Range(caveData.minSize, caveData.maxSize);
                if (startPosition.y < 5)
                {
                    continue;
                }

                float offsetX = fastNoise.GetPerlin(0, startPosition.y * frequency, startPosition.z * frequency) * amplitude;
                float offsetY = fastNoise.GetPerlin(startPosition.x * frequency, 0, startPosition.z * frequency) * amplitude;
                float offsetZ = fastNoise.GetPerlin(startPosition.x * frequency, startPosition.y * frequency, 0) * amplitude;

                Vector3 offset = new Vector3(offsetX, offsetY, offsetZ).normalized;
                Vector3Int offsetInt = new Vector3Int(GetCaveDirection(offset.x), GetCaveDirection(offset.y), GetCaveDirection(offset.z));
                startPosition += offsetInt;

                if (startPosition.y <= 3 || startPosition.y > chunkTerrainData.maxHeight)
                    break;

                float absOffsetX = Mathf.Abs(offsetX);
                float absOffsetY = Mathf.Abs(offsetY);
                float absOffsetZ = Mathf.Abs(offsetZ);
                if (absOffsetX > absOffsetY && absOffsetX > absOffsetZ)
                {
                    AddCaveRange(startPosition, caveRange, 1, lastType);
                    lastType = 1;
                }
                else if (absOffsetY > absOffsetX && absOffsetY > absOffsetZ)
                {
                    AddCaveRange(startPosition, caveRange, 2, lastType);
                    lastType = 2;
                }
                else if (absOffsetZ > absOffsetX && absOffsetZ > absOffsetY)
                {
                    AddCaveRange(startPosition, caveRange, 3, lastType);
                    lastType = 3;
                }
            }
        }
    }

    protected static int GetCaveDirection(float data)
    {
        if (data > 0.5f)
        {
            return 1;
        }
        else if (data < -0.5f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    protected static void AddCaveRange(Vector3Int startPosition, int caveRange, int type, int lastType)
    {
        if (type == lastType)
        {
            for (int a = -caveRange; a <= caveRange; a++)
            {
                for (int b = -caveRange; b <= caveRange; b++)
                {

                    Vector3Int blockPosition;
                    switch (type)
                    {
                        //x
                        case 1:
                            blockPosition = new Vector3Int(startPosition.x, startPosition.y + a, startPosition.z + b);
                            break;
                        //y
                        case 2:
                            blockPosition = new Vector3Int(startPosition.x + a, startPosition.y, startPosition.z + b);
                            break;
                        //z
                        case 3:
                            blockPosition = new Vector3Int(startPosition.x + a, startPosition.y + b, startPosition.z);
                            break;

                        default:
                            blockPosition = new Vector3Int(startPosition.x, startPosition.y, startPosition.z);
                            break;
                    }
                    float disTemp = Vector3Int.Distance(blockPosition, startPosition);
                    if (blockPosition.y <= 3 || disTemp >= caveRange - 0.5f)
                    {
                        continue;
                    }
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockPosition, BlockTypeEnum.None);
                }
            }
        }
        else
        {
            for (int a = -caveRange; a <= caveRange; a++)
            {
                for (int b = -caveRange; b <= caveRange; b++)
                {
                    for (int c = -caveRange; c <= caveRange; c++)
                    {
                        Vector3Int blockPosition = new Vector3Int(startPosition.x + a, startPosition.y + b, startPosition.z + c);
                        float disTemp = Vector3Int.Distance(blockPosition, startPosition);
                        if (blockPosition.y <= 3 || disTemp >= caveRange - 0.5f)
                        {
                            continue;
                        }
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(blockPosition, BlockTypeEnum.None);
                    }
                }
            }
        }

    }
}