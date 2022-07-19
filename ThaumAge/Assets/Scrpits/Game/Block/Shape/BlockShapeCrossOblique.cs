using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCrossOblique : BlockShapeCross
{
    public static Vector3[] VertsAddCrossOblique = new Vector3[]
    {
            new Vector3(0.5f,0f,0f),
            new Vector3(0.5f,1f,0f),
            new Vector3(0.5f,1f,1f),
            new Vector3(0.5f,0f,1f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(1f,1f,0.5f),
            new Vector3(1f,0f,0.5f)
    };

    public BlockShapeCrossOblique() : base()
    {
        vertsAdd = VertsAddCrossOblique;
    }

}