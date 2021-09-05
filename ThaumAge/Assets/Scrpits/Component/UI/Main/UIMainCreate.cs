using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class UIMainCreate : BaseUIComponent
{
    public int userDataIndex;

    public override void Awake()
    {
        base.Awake();
        ui_RRotate.AddLongClickListener(OnLongClickForRoateR);
        ui_LRotate.AddLongClickListener(OnLongClickForRoateL);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Back)
        {
            HandleForBack();
        }
    }

    /// <summary>
    /// 设置用户数据序号
    /// </summary>
    /// <param name="userDataIndex"></param>
    public void SetUserDataIndex(int userDataIndex)
    {
        this.userDataIndex = userDataIndex;
    }

    /// <summary>
    /// 处理-返回
    /// </summary>
    public void HandleForBack()
    {
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainUserData>(UIEnum.MainUserData);
        //还原摄像头
        SceneMainHandler.Instance.ChangeCameraByIndex(0);
        //还原角色角度
        SceneMainHandler.Instance.CharacterResetRotate();
    }

    /// <summary>
    /// 处理-角色旋转
    /// </summary>
    /// <param name="direction"></param>
    public void HandleForCharacterRotate(DirectionEnum direction)
    {
        SceneMainHandler.Instance.RotateCharacter(userDataIndex, direction);
    }

    #region 左右长按旋转
    public void OnLongClickForRoateR()
    {
        HandleForCharacterRotate(DirectionEnum.Right);
    }

    public void OnLongClickForRoateL()
    {
        HandleForCharacterRotate(DirectionEnum.Left);
    }
    #endregion
}