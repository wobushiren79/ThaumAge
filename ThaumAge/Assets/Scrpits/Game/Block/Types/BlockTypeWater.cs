using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockTypeWater : BlockBaseLiquid
{
    public override bool CheckIsSameType(Chunk closeChunk, Block closeBlock)
    {
        return CheckIsSameTypeWater(closeChunk, closeBlock);
    }

    public static bool CheckIsSameTypeWater(Chunk closeChunk, Block closeBlock)
    {
        switch (closeBlock.blockType)
        {
            case BlockTypeEnum.Water:
            case BlockTypeEnum.CoralRed:
            case BlockTypeEnum.CoralBlue:
            case BlockTypeEnum.CoralYellow:
            case BlockTypeEnum.Seaweed:
                return true;
            default:
                return false;
        }
    }

    public override void OnCollisionForPlayerCamera(Camera camera, Vector3Int worldPosition)
    {
        CameraHandler.Instance.SetCameraUnderLiquid(1);
    }

    public override void OnCollision(CreatureTypeEnum creatureType, GameObject targetObj, Vector3Int worldPosition, DirectionEnum direction)
    {
        if (creatureType == CreatureTypeEnum.Player && direction == DirectionEnum.None)
        {
            GameControlHandler.Instance.manager.controlForPlayer.ChangeGroundType(1);

            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterBean characterData = userData.characterData;
            CreatureStatusBean creatureStatus = characterData.GetCreatureStatus();

            CreatureStatusChangeBean creatureStatusChange = new CreatureStatusChangeBean(CreatureStatusChangeTypeEnum.MoveSpeedAdd, 1.01f, -0.3f);
            creatureStatus.AddStatusChange(creatureStatusChange);
        }
    }
}