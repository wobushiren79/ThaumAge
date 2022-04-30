using UnityEditor;
using UnityEngine;

public class PlayerRay : PlayerBase
{

    public PlayerRay(Player player) : base(player)
    {
    }


    /// <summary>
    /// 检测已摄像头未起点的角色前方的方块
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetBlockPosition"></param>
    /// <returns></returns>
    public bool RayToChunkBlock(out RaycastHit hit, out Vector3Int targetPosition)
    {
        hit = new RaycastHit();
        targetPosition = Vector3Int.zero;

        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        ControlForCamera controlForCamera = GameControlHandler.Instance.manager.controlForCamera;
        float disMax;
        float disRayBlock = 4;
        if (controlForCamera.cameraDistance <= 0)
        {
             disMax = Vector3.Distance(cameraPosition, player.objFirstLook.transform.position);
        }
        else
        {
             disMax = Vector3.Distance(cameraPosition, player.objThirdLook.transform.position);
        }
  
        //发射射线检测
        RayUtil.RayAllToScreenPointForScreenCenter(disMax + disRayBlock, 1 << LayerInfo.ChunkTrigger | 1 << LayerInfo.ChunkCollider, out RaycastHit[] arrayHit);
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
            if (disHit <= disMax)
            {
                continue;
            }
            else
            {
                //选取前方最近的点
                if (disHit < disTemp)
                {
                    disTemp = disHit;
                    hit = itemHit;
                    hasHitData = true;
                }
            }
        }
        GetHitPositionAndDirection(hit,  out targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);
        return hasHitData;
    }


    /// <summary>
    /// 获取碰撞的位置和方向
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetPosition"></param>
    /// <param name="closePosition"></param>
    /// <param name="direction"></param>
    public void GetHitPositionAndDirection(RaycastHit hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction)
    {
        targetPosition = Vector3Int.zero;
        closePosition = Vector3Int.zero;
        direction = BlockDirectionEnum.UpForward;

        Vector3 face = Vector3.Normalize(player.transform.position - hit.point);
        int rotate = Mathf.Abs(face.x) > Mathf.Abs(face.z) ? 0 : 1;
        int rotateDirection;
        if (rotate == 0)
        {
            if (face.x >= 0)
            {
                rotateDirection = 1;
            }
            else
            {
                rotateDirection = 2;
            }
        }
        else
        {
            if (face.z >= 0)
            {
                rotateDirection = 4;
            }
            else
            {
                rotateDirection = 3;
            }
        }
        if (hit.normal.y > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y - 0.01f), Mathf.FloorToInt(hit.point.z));
            closePosition = targetPosition + Vector3Int.up;

            direction = (BlockDirectionEnum)(rotateDirection + 10);
        }
        else if (hit.normal.y < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y + 0.01f), Mathf.FloorToInt(hit.point.z));
            closePosition = targetPosition + Vector3Int.down;
            direction = (BlockDirectionEnum)(rotateDirection + 20);
        }
        else if (hit.normal.x > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x - 0.01f), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
            closePosition = targetPosition + Vector3Int.right;
            direction = (BlockDirectionEnum)(rotateDirection + 40);
        }
        else if (hit.normal.x < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x + 0.01f), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
            closePosition = targetPosition + Vector3Int.left;
            direction = (BlockDirectionEnum)(rotateDirection + 30);
        }
        else if (hit.normal.z > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z - 0.01f));
            closePosition = targetPosition + Vector3Int.forward;
            direction = (BlockDirectionEnum)(rotateDirection + 50);
        }
        else if (hit.normal.z < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z + 0.01f));
            closePosition = targetPosition + Vector3Int.back;
            direction = (BlockDirectionEnum)(rotateDirection + 60);
        }
    }


}