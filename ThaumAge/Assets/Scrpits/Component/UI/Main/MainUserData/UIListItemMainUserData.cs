using System;
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
        SetUIText();
        SetDataState();
        if (userData == null)
            return;
        SetName(userData.characterData.characterName);
        SetGameTime(userData.timeForGame);
        SetPlayTime(userData.timeForPlay);
        SetSeed(userData.seed);
    }

    /// <summary>
    /// 设置UI文本
    /// </summary>
    public void SetUIText()
    {
        ui_NullText.text = TextHandler.Instance.GetTextById(201);
        ui_CreateText.text = TextHandler.Instance.GetTextById(202);
        ui_ContinueText.text = TextHandler.Instance.GetTextById(203);

        ui_NameTitle.text = TextHandler.Instance.GetTextById(204);
        ui_GameTimeTitle.text = TextHandler.Instance.GetTextById(205);
        ui_PlayTimeTitle.text = TextHandler.Instance.GetTextById(206);

        ui_SeedTitle.text = TextHandler.Instance.GetTextById(207);
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    /// <param name="viewButton"></param>
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
        else if (viewButton == ui_Delete)
        {
            HandleForDelete();
        }
        //播放音效
        AudioHandler.Instance.PlaySound(1);
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
            ui_Delete.gameObject.SetActive(false);
        }
        else
        {
            ui_ContentNull.gameObject.SetActive(false);
            ui_Content.gameObject.SetActive(true);

            ui_Continue.gameObject.SetActive(true);
            ui_Create.gameObject.SetActive(false);
            ui_Delete.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        ui_NameContent.text = name;
    }

    /// <summary>
    /// 设置游戏时间
    /// </summary>
    public void SetGameTime(TimeBean time)
    {
        ui_GameTimeContent.text = $"{time.year}:{time.month}:{time.hour}";
    }

    /// <summary>
    /// 设置游玩时间
    /// </summary>
    public void SetPlayTime(TimeBean time)
    {
        ui_PlayTimeContent.text = $"{time.hour}:{time.minute}";
    }

    /// <summary>
    /// 设置种子
    /// </summary>
    public void SetSeed(int seed)
    {
        ui_SeedContent.text = $"{seed}";
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void HandleForContinue()
    {
        //使用数据
        GameDataHandler.Instance.manager.UseUserData(userData);
        //改变场景
        SceneMainHandler.Instance.ChangeScene(ScenesEnum.GameScene);
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    public void HandleForDelete()
    {
        Action<DialogView, DialogBean> actionSubmit = (view, data) =>
         {
             GameDataHandler.Instance.manager.DeletGameData(userData.userId);
             UIHandler.Instance.RefreshUI();
             SceneMainHandler.Instance.manager.ShowCharacterObjByIndex(userDataIndex, false);
         };
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.content = TextHandler.Instance.GetTextById(20001);
        dialogData.actionSubmit = actionSubmit;
        UIDialogNormal dialog = UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

    /// <summary>
    /// 创建新角色
    /// </summary>
    public void HandleForCreate()
    {
        UIMainCreate uiCreate = UIHandler.Instance.OpenUIAndCloseOther<UIMainCreate>();
        uiCreate.SetUserDataIndex(userDataIndex);

        //设施摄像头
        SceneMainHandler.Instance.ChangeCameraByIndex(userDataIndex);
        //显示角色
        SceneMainHandler.Instance.manager.ShowCharacterObjByIndex(userDataIndex, true);
    }
}