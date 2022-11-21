using UnityEditor;
using UnityEngine;

public class BlockTypeResearchTable : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        UIGameResearch uiGameResearch = UIHandler.Instance.OpenUIAndCloseOther<UIGameResearch>();
    }

}