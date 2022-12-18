
using UnityEngine;
public class Test : BaseMonoBehaviour
{
    public MagicBean magicData;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Debug.Log("tEST1");
            Player player = GameHandler.Instance.manager.player;
            magicData.createPosition = player.transform.position + Vector3.up;
            magicData.direction = Camera.main.transform.forward;
            magicData.createTargetId = player.gameObject.GetInstanceID();
            MagicHandler.Instance.CreateMagic(magicData);
        }
    }


}
