using UnityEditor;
using UnityEngine;

public class BlockShapeFaceBoth : BlockShapeFace
{
    public BlockShapeFaceBoth() : base()
    {
        vertsAdd = new Vector3[]
        {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f),

            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
        };
        trisAdd = new int[]
        {
            0,1,2, 0,2,3, 4,6,5, 4,7,6
        };

        colorsAdd = new Color[]
        {
            Color.white,Color.white,Color.white,Color.white,
            Color.white,Color.white,Color.white,Color.white
        };
    }

    public override void InitData(Block block)
    {
        this.block = block;

        Vector2 uvStartPosition = GetUVStartPosition(block);

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }
}