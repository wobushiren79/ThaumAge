using UnityEditor;
using UnityEngine;

public class ItemTypeFood : Item
{
    public override void TargetUse(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        if (itemData == null)
            return;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        AttributeBean attributeData = itemsInfo.GetAttributeData();

        int addSatureation = attributeData.GetAttributeValue(AttributeTypeEnum.Saturation);
        int addHealth = attributeData.GetAttributeValue(AttributeTypeEnum.Health);
        int addMagic = attributeData.GetAttributeValue(AttributeTypeEnum.Magic);

        if (user == GameHandler.Instance.manager.player.gameObject)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CreatureStatusBean creatureStatus = userData.characterData.GetCreatureStatus();

            creatureStatus.SaturationChange(addSatureation);
            creatureStatus.HealthChange(addHealth);
            creatureStatus.MagicChange(addMagic);

            //减少道具
            userData.AddItems(itemData,-1);
            //刷新UI
            UIHandler.Instance.RefreshUI();
            //通知数据改变
            EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
            //播放音效
            AudioHandler.Instance.PlaySound(101);
        }
    }
}