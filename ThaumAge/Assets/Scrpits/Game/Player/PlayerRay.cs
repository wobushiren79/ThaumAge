using UnityEditor;
using UnityEngine;

public class PlayerRay : PlayerBase
{
    protected float disRayBlock = 2;

    public PlayerRay(Player player) : base(player)
    {
    }


    /// <summary>
    /// 检测已摄像头未起点的角色前方的方块
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetBlockPosition"></param>
    /// <returns></returns>
    public bool RayToChunkBlock(out RaycastHit hit,out Vector3 targetBlockPosition)
    {
        hit = new RaycastHit();
        targetBlockPosition = Vector3.zero;

        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        float disMax = Vector3.Distance(cameraPosition, player.transform.position);
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
        //设置方块位置
        targetBlockPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y) - 1, Mathf.FloorToInt(hit.point.z));
        return hasHitData;
    }
}