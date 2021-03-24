
using System;
using System.Collections.Generic;

[Serializable]
public class ChunkBean
{
    public List<BlockBean> listBlockData = new List<BlockBean>();
    public Vector3IntBean position;
}