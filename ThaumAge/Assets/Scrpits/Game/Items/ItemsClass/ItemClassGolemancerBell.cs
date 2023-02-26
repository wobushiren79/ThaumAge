using UnityEditor;
using UnityEngine;

public class ItemClassGolemancerBell : Item
{
    public override void UseForPlayer(Player player, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {
        if (player.playerRay.RayToBase(out RaycastHit hit, 1 << LayerInfo.Creature))
        {
            CreatureCptGolem creatureCpt = hit.collider.GetComponent<CreatureCptGolem>();
            if (creatureCpt != null)
            {
                var inputActionShift = GameControlHandler.Instance.manager.controlForPlayer.inputActionShift;
                float shiftInput = inputActionShift.ReadValue<float>();
                if (shiftInput != 0)
                {
                    //有按 shift
                    //回收傀儡
                    creatureCpt.CreateDropItems();
                    creatureCpt.DestoryCreature();
                }
                else
                {
                    //停止傀儡一切活动
                    creatureCpt.aiEntity.ChangeIntent(AIIntentEnum.GolemStandby);
                    //直接右键点击 打开UI
                    UIGameGolem uIGameGolem = UIHandler.Instance.OpenUIAndCloseOther<UIGameGolem>();
                    uIGameGolem.SetData(creatureCpt);
                }
            }
        }
        base.UseForPlayer(player, itemsData, itemUseType);
    }

    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        return true;
    }
}