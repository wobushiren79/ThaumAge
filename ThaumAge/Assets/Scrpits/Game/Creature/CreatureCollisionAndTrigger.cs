﻿using UnityEditor;
using UnityEngine;

public class CreatureCollisionAndTrigger : CreatureBase
{
    public CreatureCollisionAndTrigger(CreatureCptBase creature) : base(creature)
    {

    }

    /// <summary>
    /// 更新碰撞和触发
    /// </summary>
    public void UpdateCollisionAndTrigger()
    {
        //检测当前所在方块是什么
        Vector3 creaturePoint = creature.transform.position;
        Vector3Int targetBlockPosition = new Vector3Int
            (
            Mathf.FloorToInt(creaturePoint.x), 
            Mathf.FloorToInt(creaturePoint.y + 0.5f), 
            Mathf.FloorToInt(creaturePoint.z)
            );
        if (targetBlockPosition.y < 0 || targetBlockPosition.y >= WorldCreateHandler.Instance.manager.heightChunk)
            return;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetBlockPosition, out Block targetBlock, out Chunk targetChunk);
        if (targetBlock != null)
        {
            targetBlock.OnCollision(creature.gameObject, targetBlockPosition, DirectionEnum.None);
        }
        //检测正前方的碰撞
        int layerMask =  1 << LayerInfo.Obstacles;
        if (RayUtil.RayToCast(creaturePoint + Vector3.up * 0.5f, creature.transform.forward, 0.6f, layerMask, out RaycastHit hitForward))
        {
            GetHitPositionAndDirection(hitForward, creature.gameObject, out Vector3Int forwardBlockPosition);
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(forwardBlockPosition, out Block forwardBlock, out Chunk forwardChunk);
            if (forwardBlock != null)
            {
                forwardBlock.OnCollisionForward(creature.gameObject, targetBlockPosition, hitForward);
            }
        }
        //检测摄像头的碰撞
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 positionMainCamera = mainCamera.transform.position;
        Vector3Int targetBlockForCameraPosition = new Vector3Int
            (
            Mathf.FloorToInt(positionMainCamera.x), 
            Mathf.FloorToInt(positionMainCamera.y), 
            Mathf.FloorToInt(positionMainCamera.z)
            );
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetBlockForCameraPosition, out Block targetCameraBlock, out Chunk targetCameraChunk);
        if (targetCameraBlock != null)
        {
            targetCameraBlock.OnCollisionForPlayerCamera(mainCamera, targetBlockForCameraPosition);
        }
        
    }

    /// <summary>
    /// 获取碰撞的位置和方向
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="targetPosition"></param>
    /// <param name="closePosition"></param>
    /// <param name="direction"></param>
    public static void GetHitPositionAndDirection(RaycastHit hit, GameObject user, out Vector3Int targetPosition)
    {
        targetPosition = Vector3Int.zero;
        if (hit.normal.y > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y - 0.01f), Mathf.FloorToInt(hit.point.z));
        }
        else if (hit.normal.y < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y + 0.01f), Mathf.FloorToInt(hit.point.z));
        }
        else if (hit.normal.x > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x - 0.01f), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
        }
        else if (hit.normal.x < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x + 0.01f), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
        }
        else if (hit.normal.z > 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z - 0.01f));
        }
        else if (hit.normal.z < 0)
        {
            targetPosition = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z + 0.01f));
        }
    }
}