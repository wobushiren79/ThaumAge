using UnityEditor;
using UnityEngine;

public class ItemClassElementalCrystalSeed : ItemTypeBlock
{
    /// <summary>
    /// 检测是否能种植
    /// </summary>
    /// <returns></returns>
    public override bool TargetUseForCheckCanUse(Vector3Int targetPosition, Vector3Int closePosition, Block targetBlock, Block closeBlock)
    {       
        //首先获取下方方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition + Vector3Int.down, out Block downBlock, out BlockDirectionEnum downBlockDirection, out Chunk downChunk);
        //只有苔藓石 和 石头能种
        bool checkDownBlock = BlockTypeElementalCrystalSeed.CheckDownBlock(downBlock);
        return checkDownBlock;
    }
}