using UnityEditor;
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
        Vector3Int targetBlockPosition = new Vector3Int(Mathf.FloorToInt(creaturePoint.x), Mathf.FloorToInt(creaturePoint.y + 0.5f), Mathf.FloorToInt(creaturePoint.z));
        if (targetBlockPosition.y < 0 || targetBlockPosition.y > WorldCreateHandler.Instance.manager.heightChunk)
            return;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetBlockPosition, out Block targetBlock, out Chunk targetChunk);
        if (targetBlock != null)
        {
            targetBlock.OnCollision(DirectionEnum.None, creature.gameObject);
        }
        //检测正前方的碰撞
        int layerMask = 1 << LayerInfo.Chunk | 1 << LayerInfo.ChunkTrigger | 1 << LayerInfo.ChunkCollider;
        if (RayUtil.RayToCast(creaturePoint + Vector3.up * 0.5f, creature.transform.forward, 1.2f, layerMask, out RaycastHit hitForward))
        {
            Vector3Int forwardBlockPosition = new Vector3Int(Mathf.FloorToInt(hitForward.point.x), Mathf.FloorToInt(hitForward.point.y), Mathf.FloorToInt(hitForward.point.z));
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(forwardBlockPosition, out Block forwardBlock, out Chunk forwardChunk);
            if (forwardBlock != null)
            {
                forwardBlock.OnCollision(DirectionEnum.Forward, creature.gameObject);
            }
        }
    }
}