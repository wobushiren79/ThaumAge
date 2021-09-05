using UnityEditor;
using UnityEngine;

public class SceneMainHandler : SceneBaseHandler<SceneMainHandler, SceneMainManager>
{

    /// <summary>
    /// 改变摄像头
    /// </summary>
    /// <param name="index"></param>
    public void ChangeCameraByIndex(int index)
    {
        CameraHandler.Instance.ChangeCameraPriority(manager.cameraPositionStart, 0);
        CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_1, 0);
        CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_2, 0);
        CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_3, 0);
        switch (index)
        {
            case 1:
                CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_1, 200);
                break;
            case 2:
                CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_2, 200);
                break;
            case 3:
                CameraHandler.Instance.ChangeCameraPriority(manager.cameraPosition_3, 200);
                break;
            default:
                CameraHandler.Instance.ChangeCameraPriority(manager.cameraPositionStart, 200);
                break;
        }
    }

    /// <summary>
    /// 重置角色旋转角度
    /// </summary>
    public void CharacterResetRotate(int index)
    {
        GameObject characterObj = manager.GetCharacterObjByIndex(index);
        characterObj.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 重置所有角色旋转
    /// </summary>
    public void CharacterResetRotate()
    {
        CharacterResetRotate(1);
        CharacterResetRotate(2);
        CharacterResetRotate(3);
    }

    /// <summary>
    /// 旋转角色
    /// </summary>
    /// <param name="index"></param>
    public void RotateCharacter(int index, DirectionEnum direction)
    {
        GameObject characterObj = manager.GetCharacterObjByIndex(index);
        SOGameInitBean gameInitData = GameHandler.Instance.manager.gameInitData;
        if (direction == DirectionEnum.Left)
        {
            characterObj.transform.localEulerAngles += new Vector3(0, gameInitData.speedForCreateCharacterRotate * Time.deltaTime, 0);
        }
        else
        {
            characterObj.transform.localEulerAngles += new Vector3(0, -gameInitData.speedForCreateCharacterRotate * Time.deltaTime, 0);
        }
    }

}