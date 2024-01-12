using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Terrain3DCShaderBean
{
    //����λ��
    public Vector3 chunkPosition;
    //�����
    public int chunkSizeW;
    //�����
    public int chunkSizeH; 
    //��Ѩ״̬ 0�����ɶ�Ѩ 1������ͨ��Ѩ
    public int stateCaves; 
    //����״̬ 0�����ɻ��� 1���ɻ��� Y=0ʱ
    public int stateBedrock;
    
    //����
    public int seed;
    //����ƫ��
    public Vector3 seedOffset = Vector3.zero;

    //noise�㼶
    public ComputeBuffer noiseLayersArrayBuffer;
    public Terrain3DCShaderNoiseLayer[] noiseLayers;

    //��ʯ����
    public ComputeBuffer oreDatasArrayBuffer;
    public Terrain3DShaderOreData[] oreDatas;

    //��������
    public ComputeBuffer blockArrayBuffer;
    //���ǿ������������
    public ComputeBuffer blockCountBuffer;
    

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    public int GetBlockTotalNum()
    {
        return chunkSizeW * chunkSizeW * chunkSizeH;
    }

    public void Dispose()
    {
        blockArrayBuffer?.Dispose();
        blockCountBuffer?.Dispose();
    }
}

[System.Serializable]
public struct Terrain3DCShaderNoiseLayer
{
    //��̬ID
    public int biomeId;
    //����Ƶ�� ��ֵԽ�� ����Խ��
    public float frequency;
    //��� ��ֵԽ�� Խ�� ��0-1��
    public float amplitude;
    //��϶��
    public float lacunarity;
    //����ѭ���������� ���Ӷ�
    public int octaves;

    //��Ѩ�߶�
    public int caveMinHeight;
    public int caveMaxHeight;
    //��Ѩ��С
    public float caveScale;
    //��Ѩ����ֵ��0-1��
    public float caveThreshold;

    //��Ѩ����Ƶ�� ��ֵԽ�� ����Խ��
    public float caveFrequency;
    //��Ѩ��� ��ֵԽ�� Խ��
    public float caveAmplitude;
    //��Ѩѭ���������� ���Ӷ�
    public int caveOctaves;

    //�������͸߶�
    public int groundMinHeigh;
    //����߶�
    public int oceanMinHeight;
    public int oceanMaxHeight;
    //ˮ�Ĵ�С
    public float oceanScale;
    //ˮ��Ԥ��
    public float oceanThreshold;
    //ˮ����� 
    public float oceanAmplitude;
    //ˮ��Ƶ�� 
    public float oceanFrequency;
}

[System.Serializable]
public struct Terrain3DShaderOreData 
{
    //��ʯID
    public int oreId;
    //��ʯ�ܶ�
    public float oreDensity;

    //��ʯ�ķ�Χ
    public int oreMinHeight;
    public int oreMaxHeight;
}

