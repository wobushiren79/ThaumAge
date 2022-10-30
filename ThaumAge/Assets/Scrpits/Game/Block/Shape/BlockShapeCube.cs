using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class BlockShapeCube : BlockShape
{
    public static Vector3[] vertsAddLeft = new Vector3[]
    {
            new Vector3(0,1,1),
            new Vector3(0,1,0),
            new Vector3(0,0,0),
            new Vector3(0,0,1)
    };
    public static Vector3[] vertsAddRight = new Vector3[]
    {
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,1,1),
            new Vector3(1,0,1)
    };
    public static Vector3[] vertsAddDown = new Vector3[]
    {
            new Vector3(1,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,0),
            new Vector3(1,0,0)
    };
    public static Vector3[] vertsAddUp = new Vector3[]
    {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0)
    };
    public static Vector3[] vertsAddForward = new Vector3[]
    {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
            new Vector3(1,0,0)
    };
    public static Vector3[] vertsAddBack = new Vector3[]
    {
            new Vector3(1,1,1),
            new Vector3(0,1,1),
            new Vector3(0,0,1),
            new Vector3(1,0,1)
    };

    public static Color[] colorsAddCube = new Color[]
    {
            Color.white,
            Color.white,
            Color.white,
            Color.white
    };

    public static int[] trisAddCube = new int[]
    {
         0,1,2, 0,2,3
    };

    //正方形的方块 所用数据
    public Vector2[] uvsAddLeft;
    public Vector2[] uvsAddRight;
    public Vector2[] uvsAddDown;
    public Vector2[] uvsAddUp;
    public Vector2[] uvsAddForward;
    public Vector2[] uvsAddBack;

    public BlockShapeCube(Block block) : base(block)
    {
        trisAdd = trisAddCube;
        colorsAdd = colorsAddCube;

        Vector2 uvStart = GetUVStartPosition(block, DirectionEnum.Left);
        uvsAddLeft = new Vector2[]
        {
            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Right);
        uvsAddRight = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Down);
        uvsAddDown = new Vector2[]
        {
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.UP);
        uvsAddUp = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Forward);
        uvsAddForward = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Back);
        uvsAddBack = new Vector2[]
        {
            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y)
        };
    }

    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        //只有在能旋转的时候才去查询旋转方向
        BlockDirectionEnum direction = BlockDirectionEnum.UpForward;
        if (block.blockInfo.rotate_state != 0)
        {
            direction = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
        }
        //Left
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, colorsAddCube, trisAddCube);
        }

        //Right
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, colorsAddCube, trisAddCube);
        }

        //Bottom
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, colorsAddCube, trisAddCube);
        }

        //Top
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, colorsAddCube, trisAddCube);
        }

        //Forward
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, colorsAddCube, trisAddCube);
        }

        //Back
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, colorsAddCube, trisAddCube);
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    public virtual void BuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, int[] trisAdd)
    {
        BaseAddTris(chunk, localPosition, direction, face, trisAdd);
        BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
    }

    public virtual void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, int[] trisAdd)
    {
        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisNormal = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        AddTris(index, trisNormal, trisAdd);

        if (block.blockInfo.collider_state == 1)
        {
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisAdd);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisAdd);
        }
    }

    public virtual void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
    Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        AddVertsUVsColors(localPosition, direction,
            chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
            vertsAdd, uvsAdd, colorsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsAdd);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsAdd);
    }

    /// <summary>
    /// 获取起始UV
    /// </summary>
    /// <param name="buildDirection"></param>
    /// <returns></returns>
    public virtual Vector2 GetUVStartPosition(Block block, DirectionEnum buildDirection)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (arrayUVData.Length == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        else if (arrayUVData.Length == 3)
        {
            //3种面  上 中 下
            switch (buildDirection)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[2].y, uvWidth * arrayUVData[2].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[1].y, uvWidth * arrayUVData[1].x);
                    break;
            }
        }
        else if (arrayUVData.Length == 6)
        {
            //6种面  上 中 下
            switch (buildDirection)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[1].y, uvWidth * arrayUVData[1].x);
                    break;
                case DirectionEnum.Left:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[2].y, uvWidth * arrayUVData[2].x);
                    break;
                case DirectionEnum.Right:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[3].y, uvWidth * arrayUVData[3].x);
                    break;
                case DirectionEnum.Forward:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[4].y, uvWidth * arrayUVData[4].x);
                    break;
                case DirectionEnum.Back:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[5].y, uvWidth * arrayUVData[5].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
                    break;
            }
        }
        else
        {
            uvStartPosition = Vector2.zero;
        }
        return uvStartPosition;
    }


    /// <summary>
    /// 获取完整的mesh数据
    /// </summary>
    /// <returns></returns>
    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertsAddLeft
            .Concat(vertsAddRight)
            .Concat(vertsAddUp)
            .Concat(vertsAddDown)
            .Concat(vertsAddForward)
            .Concat(vertsAddBack)
            .ToArray();

        mesh.triangles = trisAdd
            .Concat(trisAdd.Add(4))
            .Concat(trisAdd.Add(8))
            .Concat(trisAdd.Add(12))
            .Concat(trisAdd.Add(16))
            .Concat(trisAdd.Add(20))
            .ToArray();
        mesh.uv = uvsAddLeft
             .Concat(uvsAddRight)
             .Concat(uvsAddUp)
             .Concat(uvsAddDown)
             .Concat(uvsAddForward)
             .Concat(uvsAddBack)
             .ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}