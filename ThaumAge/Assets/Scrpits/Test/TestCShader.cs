
using System.Diagnostics;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;

public class TestCShader : BaseMonoBehaviour
{
    public ComputeShader computeShader;
    public int objNumber = 50;
    public float perlinFrequency = 1;
    public float perlinAmplitude = 1;
    public float perlinSize = 1024;
    public int perlinIterateNumber = 3;
    public Vector2 randomOffset;
    GameObject[,] objs;
    struct Perlin2DData
    {     
        //数据
        public float perlinData;
        //当前坐标
        public Vector2 position;
        //频率（宽度）
        public float perlinFrequency;
        //振幅(高度) 
        public float perlinAmplitude;
        //循环大小 最好大于512
        public float perlinSize;
        //迭代次数（越多地图越复杂）
        public int perlinIterateNumber;

        public float offsetDis;
    }

    protected int kernelId;
    public void Start()
    {
        kernelId = computeShader.FindKernel("CSMain");
        objs = new GameObject[objNumber, objNumber];
        for (int x=0;x< objNumber;x++)
        {
            for (int z = 0; z < objNumber; z++)
            {
                GameObject objItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                objs[x, z] = objItem;
                objItem.transform.position=new Vector3(x,0,z);
            }
        }
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Test"))
        {
            HandleForTest();
        }
    }

    public void HandleForTest()
    {
        Perlin2DData[] arrayData = new Perlin2DData[objNumber * objNumber];
        float[] arrayBiomeCenterPosition = new float[]{ 1,2,3,0,4,5,6,0 };
        for (int x = 0; x < objNumber; x++)
        {
            for (int z = 0; z < objNumber; z++)
            {
                Perlin2DData perlin2DData = new Perlin2DData
                {
                    position = new Vector2(x, z),
                    perlinFrequency = this.perlinFrequency,
                    perlinAmplitude = this.perlinAmplitude,
                    perlinSize = this.perlinSize,
                    perlinIterateNumber = this.perlinIterateNumber
                };
                arrayData[x * objNumber + z] = perlin2DData;
            }
        }
        ComputeBuffer buffer = new ComputeBuffer(arrayData.Length, 32);
        buffer.SetData(arrayData);
        computeShader.SetFloats("RandomOffset", randomOffset.x, randomOffset.y);
        computeShader.SetBuffer(kernelId, "BufferTerraninData", buffer);
        computeShader.SetFloats("ArrayBiomeCenterPosition", arrayBiomeCenterPosition);
        computeShader.Dispatch(kernelId, arrayData.Length, 1, 1);

        buffer.GetData(arrayData);
        for (int x = 0; x < objNumber; x++)
        {
            for (int z = 0; z < objNumber; z++)
            {
                Perlin2DData itemData= arrayData[x * objNumber + z];
                GameObject objItem = objs[x, z];
                objItem.transform.position = new Vector3(x, itemData.perlinData, z);
                LogUtil.Log("itemData:"+ itemData.offsetDis);
            }
        }
        buffer.Dispose();
    }

}

