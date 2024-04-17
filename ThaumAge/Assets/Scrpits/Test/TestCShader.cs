
using System.Diagnostics;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class TestCShader : BaseMonoBehaviour
{
    public ComputeShader computeShader;
    public int count = 10;
    public int seed = 1123;

    Color[] colors;

    struct Perlin2DData
    {
        //当前坐标
        public Vector2 position;
        public int seed;
        public int maxBiomeNum;
        public float sideLength;
        public float sideHeight;
        public int randomData;
    }

    protected int kernelId;
    public void Start()
    {
        colors = new Color[100];
        for (int i = 0; i < 100; i++)
        {
            colors[i] = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        }
        kernelId = computeShader.FindKernel("CSMain");
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            HandleForTest2();
        }
    }

    [System.Serializable]
    public struct VoxelDetails
    {
        public float color;
        public float metallic;
        public float smoothness;
    }
    public struct BlockData
    {
        public int blockId;
    }

    public ComputeShader noiseShader;
    public ComputeBuffer noiseLayersArray;
    //public ComputeShader voxelShader;

    public Terrain3DCShaderNoiseLayer[] noiseLayers;

    public int chunkSize = 16;
    public int maxHeight = 256;
    public bool isUseTextures = false;
    public bool isSharedVertices = false;
    public Vector3 chunkPosition = Vector3.zero;


    public void HandleForTest2()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Terrain3DCShaderBean terrain3DCShaderBean = new Terrain3DCShaderBean();
        terrain3DCShaderBean.chunkPosition = chunkPosition;
        terrain3DCShaderBean.chunkSizeW = chunkSize;
        terrain3DCShaderBean.chunkSizeH = maxHeight;
        terrain3DCShaderBean.stateCaves = 1;
        terrain3DCShaderBean.stateBedrock = 1;
        terrain3DCShaderBean.seed = seed;
        terrain3DCShaderBean.seedOffset = Vector3.zero;
        terrain3DCShaderBean.noiseLayers = noiseLayers;
        CShaderHandler.Instance.HandleTerrain3DCShader(BiomeTypeEnum.Test, terrain3DCShaderBean, (shaderData)=> {

            stopwatch.Stop();
            LogUtil.Log($"stopwatch1 {stopwatch.ElapsedTicks}");
            stopwatch.Restart();

            BlockData[] blockArray = new BlockData[terrain3DCShaderBean.GetBlockTotalNum()];
            shaderData.blockArrayBuffer.GetData(blockArray);

            uint[] count = new uint[1];
            shaderData.blockCountBuffer.GetData(count);
            stopwatch.Stop();
            LogUtil.Log($"stopwatch2 {stopwatch.ElapsedTicks}");

            LogUtil.Log($"Length {blockArray.Length}");
            LogUtil.Log($"count {count[0]}");

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        var itemData = blockArray[x + (y * chunkSize) + (z * chunkSize * maxHeight)];
                        if (itemData.blockId == 0)
                        {
                            continue;
                        }
                        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        obj.transform.position = new Vector3(x, y, z) + chunkPosition;
                    }
                }
            }
        });
    }

    VoxelDetails[] getVoxelDetails()
    {
        VoxelDetails[] voxelDetails = new VoxelDetails[16];
        return voxelDetails;
    }

    public void HandleForTest()
    {
        Perlin2DData[] arrayData = new Perlin2DData[count * count];
        for (int x = 0; x < count; x++)
        {
            for (int z = 0; z < count; z++)
            {
                Perlin2DData perlin2DData = new Perlin2DData
                {
                    position = new Vector2(x, z),
                    seed = this.seed,
                    maxBiomeNum = colors.Length
                };
                arrayData[x * count + z] = perlin2DData;
            }
        }

        ComputeBuffer buffer = new ComputeBuffer(arrayData.Length, 28);
        buffer.SetData(arrayData);
        //computeShader.SetFloats("RandomOffset", randomOffset.x, randomOffset.y);
        computeShader.SetBuffer(kernelId, "BufferTerraninData", buffer);
        //computeShader.SetFloats("ArrayBiomeCenterPosition", arrayBiomeCenterPosition);
        computeShader.Dispatch(kernelId, arrayData.Length, 1, 1);

        buffer.GetData(arrayData);
        for (int x = 0; x < count; x++)
        {
            for (int z = 0; z < count; z++)
            {
                Perlin2DData itemData = arrayData[x * count + z];
                LogUtil.Log("itemData:" + itemData.randomData + "");
            }
        }
        buffer.Dispose();
    }

}

