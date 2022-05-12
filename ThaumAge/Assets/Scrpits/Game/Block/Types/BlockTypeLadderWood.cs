using UnityEditor;
using UnityEngine;

public class BlockTypeLadderWood : Block
{
    public override void OnCollisionForward(GameObject user, Vector3Int worldPosition, RaycastHit raycastHit)
    {
        base.OnCollisionForward(user, worldPosition, raycastHit);
        if (raycastHit.collider.gameObject.layer == LayerInfo.Obstacles)
        {
            ControlForPlayer controlForPlayer = user.transform.GetComponentInParent<ControlForPlayer>();
            if (controlForPlayer != null)
            {
                controlForPlayer.HanldeForClimb();
            }
        }
    }

}