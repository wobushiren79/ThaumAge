using UnityEditor;
using UnityEngine;

public class BlockTypeStairsWood : Block
{
    public override void OnCollision(DirectionEnum direction, GameObject user)
    {
        base.OnCollision(direction, user);
        if(direction == DirectionEnum.Forward)
        {
            ControlForPlayer controlForPlayer = user.transform.GetComponentInParent<ControlForPlayer>();
            if (controlForPlayer != null)
            {
                controlForPlayer.HanldeForClimb();
            }
        }
    }
}