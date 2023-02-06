using UnityEditor;
using UnityEngine;

public class BlockTypeArcaneLevitatorComponent : BlockTypeComponent
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerInfo.Character)
        {
            Player player = GameHandler.Instance.manager.player;
            if (other.gameObject == player.gameObject)
            {
                ControlForPlayer controlForPlayer = GameControlHandler.Instance.manager.controlForPlayer;
                controlForPlayer.ChangeGroundType(2);
            }
        }
    }
}