
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkBean
{
    public Vector3IntBean position;
    public Dictionary<string, BlockBean> dicBlockData = new Dictionary<string, BlockBean>();

}