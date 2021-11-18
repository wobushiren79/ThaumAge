using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCrossOblique : BlockShapeCross
{
    public BlockShapeCrossOblique()
    {
        vertsAdd = new Vector3[]
        {
            new Vector3(0f,0f,0f),
            new Vector3(0f,1f,0f),
            new Vector3(1f,1f,1f),
            new Vector3(1f,0f,1f),

            new Vector3(1f,0f,0f),
            new Vector3(1f,1f,0f),
            new Vector3(0f,1f,1f),
            new Vector3(0f,0f,1f)
        };
    }

}