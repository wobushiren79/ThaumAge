using UnityEditor;
using UnityEngine;

public class ChunkDataBean
{
    public Chunk _chunkLeft;
    public Chunk chunkLeft
    {
        get
        {
            if (_chunkLeft == null)
            {
                _chunkLeft = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(-chunkWidth, 0, 0));
            }
            return _chunkLeft;
        }
    }
    public Chunk _chunkRight;
    public Chunk chunkRight
    {
        get
        {
            if (_chunkRight == null)
            {
                _chunkRight = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(chunkWidth, 0, 0));
            }
            return _chunkRight;
        }
    }
    public Chunk _chunkForward;
    public Chunk chunkForward
    {
        get
        {
            if (_chunkForward == null)
            {
                _chunkForward = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, -chunkWidth));
            }
            return _chunkForward;
        }
    }
    public Chunk _chunkBack;
    public Chunk chunkBack
    {
        get
        {
            if (_chunkBack == null)
            {
                _chunkBack = WorldCreateHandler.Instance.manager.GetChunk(positionForWorld + new Vector3Int(0, 0, chunkWidth));
            }
            return _chunkBack;
        }
    }

    //所有的方块合集
    public Block[] arrayBlock;
    //所有方块的方向集合
    public byte[] arrayBlockDirection;

    //世界坐标
    public Vector3Int positionForWorld;

    public int chunkWidth;
    public int chunkHeight;

    public ChunkDataBean(Vector3Int wPosition, int chunkWidth, int chunkHeight)
    {
        this.chunkWidth = chunkWidth;
        this.chunkHeight = chunkHeight;
        this.positionForWorld = wPosition;

        arrayBlock = new Block[chunkWidth * chunkHeight * chunkWidth];
        arrayBlockDirection = new byte[chunkWidth * chunkHeight * chunkWidth];
    }

    /// <summary>
    /// 设置方块
    /// </summary>
    public void SetBlockForLocal(int x, int y, int z, Block block, byte direction)
    {
        int index = GetIndexByPosition(x, y, z);
        arrayBlock[index] = block;
        arrayBlockDirection[index] = direction;
    }

    public void SetBlockForLocal(int x, int y, int z, Block block, DirectionEnum direction)
    {
        SetBlockForLocal(x, y, z, block, (byte)direction);
    }
    public void SetBlockForLocal(Vector3Int blockPosition, Block block, byte direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, block, direction);
    }
    public void SetBlockForLocal(Vector3Int blockPosition, Block block, DirectionEnum direction)
    {
        SetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, block, (byte)direction);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, Block block)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, block, DirectionEnum.UP);
    }

    public void SetBlockForWorld(Vector3Int worldPosition, Block block, DirectionEnum direction)
    {
        Vector3Int blockLocalPosition = worldPosition - this.positionForWorld;
        SetBlockForLocal(blockLocalPosition, block, direction);
    }

    /// <summary>
    /// 获取方块
    /// </summary>
    public void GetBlockForLocal(int x, int y, int z, out Block block, out DirectionEnum direction)
    {
        int index = GetIndexByPosition(x, y, z);
        block = arrayBlock[index];
        direction = (DirectionEnum)arrayBlockDirection[index];
    }

    public void GetBlockForLocal(Vector3Int blockPosition, out Block block, out DirectionEnum direction)
    {
        GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z, out block, out direction);
    }

    public Block GetBlockForLocal(int x, int y, int z)
    {
        int index = GetIndexByPosition(x, y, z);
        return  arrayBlock[index];
    }

    public Block GetBlockForLocal(Vector3Int blockPosition)
    {
        return GetBlockForLocal(blockPosition.x, blockPosition.y, blockPosition.z);
    }

    /// <summary>
    /// 获取下标
    /// </summary>
    public int GetIndexByPosition(Vector3Int position)
    {
        return GetIndexByPosition(position.x, position.y, position.z);
    }
    public int GetIndexByPosition(int x, int y, int z)
    {
        return x * chunkWidth * chunkHeight + y * chunkWidth + z;
    }
}