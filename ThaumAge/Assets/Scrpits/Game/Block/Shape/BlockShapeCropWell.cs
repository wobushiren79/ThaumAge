﻿
using UnityEditor;
using UnityEngine;

public class BlockShapeCropWell : BlockShapeWell
{
    public BlockShapeCropWell() : base()
    {
        BlockBaseCrop.InitCropVert(vertsAdd);
    }


}