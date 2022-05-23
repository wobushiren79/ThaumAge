using UnityEditor;
using UnityEngine;

public class BlockTypeFurnacesSimple : BlockBaseFurnaces
{
    /// <summary>
    /// 打开熔炉
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);

        //打开箱子UI
        UIGameFurnacesSimple uiGameFurnacesSimple = UIHandler.Instance.OpenUIAndCloseOther<UIGameFurnacesSimple>(UIEnum.GameFurnacesSimple);
        //设置数据
        uiGameFurnacesSimple.SetFurnacesData(worldPosition);
    }
}