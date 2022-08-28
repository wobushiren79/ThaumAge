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

        if(user == GameHandler.Instance.manager.player.gameObject)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.characterData.GetCharacterStatus().SaturationChange(addSatureation);

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