using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSaveBaseBean : BaseBean
{
    //��������
    public int worldType = 0;
    //�����˺�
    public string userId;
    //chunk����
    public Vector3Int position;
    //��̬����
    public int biomeType = -1;

    public WorldTypeEnum GetWorldType()
    {
        return (WorldTypeEnum)worldType;
    }
}