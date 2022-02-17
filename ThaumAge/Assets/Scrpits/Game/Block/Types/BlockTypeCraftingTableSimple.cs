using UnityEditor;
using UnityEngine;

public class BlockTypeCraftingTableSimple : Block
{
    /// <summary>
    /// 互动
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition)
    {
        base.Interactive(user, worldPosition);
        UIGameUserDetails uiGameUserDetails = UIHandler.Instance.OpenUI<UIGameUserDetails>(UIEnum.GameUserDetails);
        uiGameUserDetails.ui_ViewSynthesis.SetDataType(ItemsSynthesisTypeEnum.Base);
        uiGameUserDetails.SetSelectType(1);
    }
}