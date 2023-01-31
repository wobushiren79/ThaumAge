using UnityEditor;
using UnityEngine;

public class BlockTypePavingStoneOfTravel : Block
{
    public override void OnCollision(CreatureTypeEnum creatureType, GameObject targetObj, Vector3Int worldPosition, DirectionEnum direction)
    {
        base.OnCollision(creatureType, targetObj, worldPosition, direction);
        if (creatureType == CreatureTypeEnum.Player && direction == DirectionEnum.Down)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterBean characterData = userData.characterData;
            CreatureStatusBean creatureStatus = characterData.GetCreatureStatus();

            CreatureStatusChangeBean creatureStatusChange = new CreatureStatusChangeBean(AttributeTypeEnum.MoveSpeedAdd, 1.01f, 0.6f);
            creatureStatus.AddStatusChange(creatureStatusChange);
        }
    }
}