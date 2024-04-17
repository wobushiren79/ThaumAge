using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TestCShader;
using UnityEngine.Rendering;

public partial class CShaderHandler
{

    /// <summary>
    /// 处理3D地形处理的Shader
    /// </summary> 
    public void HandleTerrain3DCShader(BiomeTypeEnum biomeType, Terrain3DCShaderBean cshaderData, Action<Terrain3DCShaderBean> callBackComplete)
    {
        ComputeShader targetCShader = manager.GetTerrain3DCShader(biomeType);
        //线程数量为大小除以cshader的numthreads
        int xThreads = cshaderData.chunkSizeW / 8;
        int yThreads = cshaderData.chunkSizeH / 8;

        //设置区块位置
        targetCShader.SetVector("chunkPosition", cshaderData.chunkPosition);
        //设置区块大小
        targetCShader.SetInt("chunkSizeW", cshaderData.chunkSizeW);
        targetCShader.SetInt("chunkSizeH", cshaderData.chunkSizeH);
        //设置洞穴状态
        targetCShader.SetInt("stateCaves", cshaderData.stateCaves);
        //设置基岩状态
        targetCShader.SetInt("stateBedrock", cshaderData.stateBedrock);

        //设置种子
        targetCShader.SetInt("seed", cshaderData.seed);
        targetCShader.SetVector("seedOffset", cshaderData.seedOffset);

        //设置noisebuffer
        cshaderData.noiseLayersArrayBuffer = new ComputeBuffer(cshaderData.noiseLayers.Length, 76);
        cshaderData.noiseLayersArrayBuffer.SetData(cshaderData.noiseLayers);

        targetCShader.SetBuffer(0, "noiseLayersArrayBuffer", cshaderData.noiseLayersArrayBuffer);
        targetCShader.SetInt("noiseLayersCount", cshaderData.noiseLayers.Length);

        //设置矿石buffer
        cshaderData.oreDatasArrayBuffer = new ComputeBuffer(cshaderData.oreDatas.Length, 16);
        cshaderData.oreDatasArrayBuffer.SetData(cshaderData.oreDatas);
        targetCShader.SetBuffer(0, "oreDatasArrayBuffer", cshaderData.oreDatasArrayBuffer);
        if (cshaderData.oreDatas.Length == 1 && cshaderData.oreDatas[0].oreId == 0)
        {
            targetCShader.SetInt("oreDatasCount", 0);
        }
        else
        {
            targetCShader.SetInt("oreDatasCount", cshaderData.oreDatas.Length);
        }

        //设置方块buffer
        cshaderData.blockArrayBuffer = new ComputeBuffer(cshaderData.GetBlockTotalNum(), 8);
        targetCShader.SetBuffer(0, "blockArrayBuffer", cshaderData.blockArrayBuffer);

        //设置不是空气方块的数量buffer
        cshaderData.blockCountBuffer = new ComputeBuffer(1, 4, ComputeBufferType.Counter);
        cshaderData.blockCountBuffer.SetCounterValue(0);
        cshaderData.blockCountBuffer.SetData(new uint[] { 0 });
        targetCShader.SetBuffer(0, "blockCountBuffer", cshaderData.blockCountBuffer);

        //int kernelIndex = targetCShader.FindKernel("");
        targetCShader.Dispatch(0, xThreads, yThreads, xThreads);
        AsyncGPUReadback.Request(cshaderData.blockCountBuffer, (callbackForGPU) =>
        {
            callBackComplete?.Invoke(cshaderData);
            cshaderData.Dispose();
        });
    }
}
