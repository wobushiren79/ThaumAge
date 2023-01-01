using UnityEditor;
using UnityEngine;

public class ItemTypeFood : Item
{
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;

        if (itemData == null)
            return false;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        AttributeBean attributeData = itemsInfo.GetAttributeData();

        int addSatureation = attributeData.GetAttributeValue(AttributeTypeEnum.Saturation);
        int addHealth = attributeData.GetAttributeValue(AttributeTypeEnum.Health);
        int addMana = attributeData.GetAttributeValue(AttributeTypeEnum.Mana);

        if (user == GameHandler.Instance.manager.player.gameObject)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CreatureStatusBean creatureStatus = userData.characterData.GetCreatureStatus();

            creatureStatus.SaturationChange(addSatureation);
            creatureStatus.HealthChange(addHealth);
            creatureStatus.ManaChange(addMana);

            //减少道具
            userData.AddItems(itemData,-1);
            //刷新UI
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
            //通知数据改变
            EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
            //播放音效
            AudioHandler.Instance.PlaySound(101);
        }
        return false;
    }
}