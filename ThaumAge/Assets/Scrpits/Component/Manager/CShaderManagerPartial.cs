using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CShaderManager
{
    protected ComputeShader terrain3DCShader;

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="callBackForComplete"></param>
    public void LoadResources(Action callBackForComplete)
    {
        terrain3DCShader = GetComputeShader("Terrain3DCShader");
        callBackForComplete?.Invoke();
    }

    /// <summary>
    /// 获取地形shader
    /// </summary>
    /// <returns></returns>
    public ComputeShader GetTerrain3DCShader()
    {
        return terrain3DCShader;
    }

}
