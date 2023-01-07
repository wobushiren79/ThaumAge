﻿using UnityEditor;
using UnityEngine;

public class BlockTypePavingStoneOfHealth : Block
{
    public override void OnCollision(CreatureTypeEnum creatureType, GameObject targetObj, Vector3Int worldPosition, DirectionEnum direction)
    {
        base.OnCollision(creatureType, targetObj, worldPosition, direction);
        if (creatureType == CreatureTypeEnum.Player && direction == DirectionEnum.Down)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterBean characterData = userData.characterData;
            CreatureStatusBean creatureStatus = characterData.GetCreatureStatus();

            CreatureStatusChangeBean creatureStatusChange = new CreatureStatusChangeBean(CreatureStatusChangeTypeEnum.HealthAdd, 1, 1);
            creatureStatus.AddStatusChange(creatureStatusChange);
        }
    }
}