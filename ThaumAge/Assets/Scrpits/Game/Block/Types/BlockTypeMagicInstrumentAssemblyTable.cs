using UnityEditor;
using UnityEngine;

public class BlockTypeMagicInstrumentAssemblyTable : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        var uiMagicInstrumentAssembly =  UIHandler.Instance.OpenUIAndCloseOther<UIGameMagicInstrumentAssembly>(UIEnum.GameMagicInstrumentAssembly);
        uiMagicInstrumentAssembly.SetData(worldPosition);
        AudioHandler.Instance.PlaySound(1);
    }
}