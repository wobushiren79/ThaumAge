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
    public Terrain3DCShaderNoiseLayers[] noiseLayers;

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
public struct Terrain3DCShaderNoiseLayers
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
    public int oceanHeight;
}
