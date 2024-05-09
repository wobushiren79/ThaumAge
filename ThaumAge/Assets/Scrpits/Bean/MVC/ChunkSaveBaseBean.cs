using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSaveBaseBean : BaseBean
{
    //世界类型
    public int worldType = 0;
    //所属账号
    public string userId;
    //chunk坐标
    public Vector3Int position;
    //生态数据
    public int biomeType = -1;

    public WorldTypeEnum GetWorldType()
    {
        return (WorldTypeEnum)worldType;
    }
}