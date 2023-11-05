using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TestCShader;
using UnityEngine.Rendering;

public partial class CShaderHandler
{

    /// <summary>
    /// ����3D���δ����Shader
    /// </summary> 
    public void HandleTerrain3DCShader(Terrain3DCShaderBean cshaderData, Action<Terrain3DCShaderBean> callBackComplete)
    {
        ComputeShader targetCShader = manager.GetTerrain3DCShader();

        //�߳�����Ϊ��С����cshader��numthreads
        int xThreads = cshaderData.chunkSizeW / 8;
        int yThreads = cshaderData.chunkSizeH / 8;

        //��������λ��
        targetCShader.SetVector("chunkPosition", cshaderData.chunkPosition);
        //���������С
        targetCShader.SetInt("chunkSizeW", cshaderData.chunkSizeW);
        targetCShader.SetInt("chunkSizeH", cshaderData.chunkSizeH);
        //���ö�Ѩ״̬
        targetCShader.SetInt("stateCaves", cshaderData.stateCaves);
        //���û���״̬
        targetCShader.SetInt("stateBedrock", cshaderData.stateBedrock);
        //���ú���߶�
        targetCShader.SetInt("oceanHeight", cshaderData.oceanHeight);

        //��������
        targetCShader.SetInt("seed", cshaderData.seed);
        targetCShader.SetVector("seedOffset", cshaderData.seedOffset);

        //����noisebuffer
        cshaderData.noiseLayersArrayBuffer = new ComputeBuffer(cshaderData.noiseLayers.Length, 40);
        cshaderData.noiseLayersArrayBuffer.SetData(cshaderData.noiseLayers);

        targetCShader.SetBuffer(0, "noiseLayersArrayBuffer", cshaderData.noiseLayersArrayBuffer);
        targetCShader.SetInt("noiseLayersCount", cshaderData.noiseLayers.Length);

        //���÷���buffer
        cshaderData.blockArrayBuffer = new ComputeBuffer(cshaderData.GetBlockTotalNum(), 4);
        targetCShader.SetBuffer(0, "blockArrayBuffer", cshaderData.blockArrayBuffer);

        //���ò��ǿ������������buffer
        cshaderData.blockCountBuffer = new ComputeBuffer(1, 4, ComputeBufferType.Counter);
        cshaderData.blockCountBuffer.SetCounterValue(0);
        cshaderData.blockCountBuffer.SetData(new uint[] { 0 });
        targetCShader.SetBuffer(0, "blockCountBuffer", cshaderData.blockCountBuffer);

        targetCShader.Dispatch(0, xThreads, yThreads, xThreads);
        AsyncGPUReadback.Request(cshaderData.blockCountBuffer, (callbackForGPU) =>
        {
            callBackComplete?.Invoke(cshaderData);
            cshaderData.Dispose();
        });
    }
}
