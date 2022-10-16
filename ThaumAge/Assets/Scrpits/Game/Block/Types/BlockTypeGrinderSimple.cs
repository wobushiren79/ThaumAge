using UnityEditor;
using UnityEngine;

public class BlockTypeGrinderSimple : BlockBaseGrinder
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        UIGameItemsTransition uiameItemsTransition= UIHandler.Instance.OpenUIAndCloseOther<UIGameItemsTransition>(UIEnum.GameItemsTransition);
        //设置数据
        uiameItemsTransition.SetData(worldPosition);
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }
}