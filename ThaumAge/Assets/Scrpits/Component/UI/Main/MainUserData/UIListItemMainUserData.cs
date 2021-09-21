using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIListItemMainUserData : BaseUIView
{
    protected UserDataBean userData;

    protected int userDataIndex;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="userData"></param>
    public void SetData(int userDataIndex, UserDataBean userData)
    {
        this.userDataIndex = userDataIndex;
        this.userData = userData;
        RefreshUI();
    }


    public override void RefreshUI()
    {
        base.RefreshUI();
        SetDataState();
    }


    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Create)
        {
            HandleForCreate();
        }
        else if (viewButton == ui_Continue)
        {
            HandleForContinue();
        }
    }

    /// <summary>
    /// 设置是否有数据
    /// </summary>
    public void SetDataState()
    {
        if (userData == null)
        {
            ui_ContentNull.gameObject.SetActive(true);
            ui_Content.gameObject.SetActive(false);

            ui_Continue.gameObject.SetActive(false);
            ui_Create.gameObject.SetActive(true);
        }
        else
        {
            ui_ContentNull.gameObject.SetActive(false);
            ui_Content.gameObject.SetActive(true);

            ui_Continue.gameObject.SetActive(true);
            ui_Create.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void HandleForContinue()
    {

    }

    /// <summary>
    /// 创建新角色
    /// </summary>
    public void HandleForCreate()
    {
        UIMainCreate uiCreate = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainCreate>(UIEnum.MainCreate);
        uiCreate.SetUserDataIndex(userDataIndex);

        //设施摄像头
        SceneMainHandler.Instance.ChangeCameraByIndex(userDataIndex);
        //显示角色
        SceneMainHandler.Instance.manager.ShowCharacterObjByIndex(userDataIndex, true);
    }
}