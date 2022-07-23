
using System.Diagnostics;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;

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
            HandleForTest();
        }
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

