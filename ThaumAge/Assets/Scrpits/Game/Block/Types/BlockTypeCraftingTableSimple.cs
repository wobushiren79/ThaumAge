using UnityEditor;
using UnityEngine;

public class BlockTypeCraftingTableSimple : Block
{
    /// <summary>
    /// 互动
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition,BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        //只有player才能打开
        if (user == null || user.GetComponent<Player>() == null)
            return;
        UIGameUserDetails uiGameUserDetails = UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
        uiGameUserDetails.ui_ViewSynthesis.SetDataType(ItemsSynthesisTypeEnum.Base);
        uiGameUserDetails.SetSelectType(1);
    }
}