using UnityEditor;
using UnityEngine;

public class ItemClassElementalPowderThaum : Item
{
    /// <summary>
    /// 右键使用
    /// </summary>
    public override void TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk taragetChunk);
        //如果靠近得方块有区块
        if (taragetChunk != null)
        {
            //如果不是空方块 则不放置(液体则覆盖放置)
            if (targetBlock == null)
                return;
            //将建议制造台变成奥术制造台

            switch (targetBlock.blockType)
            {
                case BlockTypeEnum.CraftingTableSimple:
                    HandleForCraftingTableArcane(targetPosition, targetBlock, targetBlockDirection, taragetChunk);
                    break;
                default:
                    return;
            }
            //扣除道具
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.AddItems(itemData, -1);
            //刷新UI
            UIHandler.Instance.RefreshUI();
            //播放音效
            AudioHandler.Instance.PlaySound(1101, targetPosition);
        }
    }

    /// <summary>
    /// 处理-奥术制造台
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetBlock"></param>
    /// <param name="targetBlockDirection"></param>
    /// <param name="taragetChunk"></param>
    protected void HandleForCraftingTableArcane(Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk taragetChunk)
    {
        EffectHandler.Instance.WaitExecuteSeconds(2, () =>
        {
            //获取目标方块
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out targetBlock, out targetBlockDirection, out taragetChunk);
            if (taragetChunk != null && targetBlock.blockType == BlockTypeEnum.CraftingTableSimple)
            {
                taragetChunk.SetBlockForLocal(targetPosition - taragetChunk.chunkData.positionForWorld, BlockTypeEnum.CraftingTableArcane, targetBlockDirection);
                //播放音效
                AudioHandler.Instance.PlaySound(3, targetPosition);
            }
        });
        //播放粒子特效
        EffectBean effectSmoke = new EffectBean();
        effectSmoke.effectType = EffectTypeEnum.Visual;
        effectSmoke.effectName = EffectInfo.Effect_Change_1;
        effectSmoke.effectPosition = new Vector3(0.5f + targetPosition.x, 0.5f + targetPosition.y, 0.5f + targetPosition.z);
        effectSmoke.timeForShow = 5;
        EffectHandler.Instance.ShowEffect(effectSmoke);
    }
}