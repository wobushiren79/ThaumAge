using UnityEditor;
using UnityEngine;

public class PlayerRay : PlayerBase
{
    protected float disRayBlock = 3;

    public PlayerRay(Player player) : base(player)
    {
    }


    /// <summary>
    /// 检测已摄像头未起点的角色前方的方块
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetBlockPosition"></param>
    /// <returns></returns>
    public bool RayToChunkBlock(out RaycastHit hit,out Vector3Int targetPosition)
    {
        hit = new RaycastHit();
        targetPosition = Vector3Int.zero;

        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        float disMax = Vector3.Distance(cameraPosition, player.objThirdLook.transform.position);

        //发射射线检测
        RayUtil.RayAllToScreenPointForScreenCenter(disMax + disRayBlock, 1 << LayerInfo.Chunk, out RaycastHit[] arrayHit);
        //如果没有发生碰撞
        if (arrayHit == null || arrayHit.Length == 0)
        {    
            return false;
        }
        bool hasHitData = false;
        float disTemp = float.MaxValue;
        for (int i = 0; i < arrayHit.Length; i++)
        {
            RaycastHit itemHit = arrayHit[i];
            float disHit = Vector3.Distance(itemHit.point, cameraPosition);
            //如果发射点到碰撞点的距离 小于 发射点到角色的距离 那说明在背后发生了碰撞 则不处理
            if(disHit <= disMax)
            {
                continue;
            }
            else
            {
                //选取前方最近的点
                if(disHit < disTemp)
                {
                    disTemp = disHit;
                    hit = itemHit;
                    hasHitData = true;
                }
            }
        }
        GetHitPositionAndDirection(hit, out targetPosition, out Vector3Int closePosition, out DirectionEnum direction);
        return hasHitData;
    }


    /// <summary>
    /// 获取碰撞的位置和方向
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetPosition"></param>
    /// <param name="closePosition"></param>
    /// <param name="direction"></param>
    public void GetHitPositionAndDirection(RaycastHit hit, out Vector3Int targetPosition, out Vector3Int closePosition, out DirectionEnum direction)
    {
        targetPosition = Vector3Int.zero;
        closePosition = Vector3Int.zero;
        direction = DirectionEnum.UP;
        if (hit.normal.y > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y) - 1, (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.up;
            direction = DirectionEnum.UP;
        }
        else if (hit.normal.y < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.down;
            direction = DirectionEnum.Down;
        }
        else if (hit.normal.x > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x) - 1, (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.right;
            direction = DirectionEnum.Right;
        }
        else if (hit.normal.x < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.left;
            direction = DirectionEnum.Left;
        }
        else if (hit.normal.z > 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z) - 1);
            closePosition = targetPosition + Vector3Int.forward;
            direction = DirectionEnum.Forward;
        }
        else if (hit.normal.z < 0)
        {
            targetPosition = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
            closePosition = targetPosition + Vector3Int.back;
            direction = DirectionEnum.Back;
        }
    }
}