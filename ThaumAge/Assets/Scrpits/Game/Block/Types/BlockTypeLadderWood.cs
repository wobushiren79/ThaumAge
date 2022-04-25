using UnityEditor;
using UnityEngine;

public class BlockTypeLadderWood : Block
{
    public override void OnCollision(GameObject user, Vector3Int worldPosition, DirectionEnum direction)
    {
        base.OnCollision(user, worldPosition, direction);
        if (direction == DirectionEnum.Forward)
        {
            ControlForPlayer controlForPlayer = user.transform.GetComponentInParent<ControlForPlayer>();
            if (controlForPlayer != null)
            {
                controlForPlayer.HanldeForClimb();
            }
        }
    }
}