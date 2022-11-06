using UnityEditor;
using UnityEngine;

public class BlockTypeMagicInstrumentAssemblyTable : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMagicInstrumentAssembly>(UIEnum.GameMagicInstrumentAssembly);
        AudioHandler.Instance.PlaySound(1);
    }
}