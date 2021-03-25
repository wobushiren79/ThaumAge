
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkBean
{
    public Vector3IntBean position;
    public Dictionary<Vector3Int, BlockBean> dicBlockData= new Dictionary<Vector3Int, BlockBean>();
}