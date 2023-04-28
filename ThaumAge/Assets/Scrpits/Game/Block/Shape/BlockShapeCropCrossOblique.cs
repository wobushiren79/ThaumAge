using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropCrossOblique : BlockShapeCropCross
{
    public static Vector3[] VertsAddCrossOblique = new Vector3[]
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

    public BlockShapeCropCrossOblique(Block block) : base(block)
    {
        vertsAdd = VertsAddCrossOblique.AddY(-(1f / 16f));
    }
}