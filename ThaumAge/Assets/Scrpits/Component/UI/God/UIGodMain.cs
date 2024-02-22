using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGodMain : UIGameCommonNormal
{

    protected List<ItemsInfoBean> listItemsInfo = new List<ItemsInfoBean>();

    public override void Awake()
    {
        base.Awake();
        ui_ItemList.AddCellListener(OnCellForItem);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        InitData();
    }
    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI(isOpenInit);
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        switch (inputType)
        {
            case InputActionUIEnum.F12:
                HandleForBackGameMain();
                break;
        }
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Time_1)
        {
            HandleForTimeChange(0, 0);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_Time_2)
        {
            HandleForTimeChange(6, 0);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_Time_3)
        {
            HandleForTimeChange(12, 0);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_Time_4)
        {
            HandleForTimeChange(18, 0);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
        else if (viewButton == ui_RefreshWorld)
        {
            WorldCreateHandler.Instance.ClearWorld();
            //如果是测试生态 刷新种子
            if (GameHandler.Instance.launcher is GameLauncher gameLauncher)
            {
                WorldCreateHandler.Instance.manager.SetWorldSeed(gameLauncher.seed);
            }
            //刷新测试生态数据
            BiomeHandler.Instance.RefreshTestBiomeData();

            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            //创建生态
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, gameConfig.worldRefreshRange, true, () => 
            {
                //初始化游戏角色
                GameHandler.Instance.InitCharacter();
            });
        }
        else if (viewButton == ui_FlyMod)
        {
            var characterData = GameHandler.Instance.manager.player.character.GetCharacterData();
            if (characterData.creatureStatus.gravityRate == 0)
            {
                //飞行模式
                GameHandler.Instance.manager.player.character.GetCharacterData().creatureStatus.gravityRate = 1;
            }
            else
            {
                //飞行模式
                GameHandler.Instance.manager.player.character.GetCharacterData().creatureStatus.gravityRate = 0;
            }       
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        listItemsInfo.Clear();
        List<ItemsInfoBean> listAllItemsInfo = ItemsHandler.Instance.manager.GetAllItemsInfo();
        for (int i = 0; i < listAllItemsInfo.Count; i++)
        {
            ItemsInfoBean itemData = listAllItemsInfo[i];
            if (itemData.id == 0)
                continue;
            listItemsInfo.Add(listAllItemsInfo[i]);
        }
        ui_ItemList.SetCellCount(listItemsInfo.Count);
    }

    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        ItemsInfoBean itemsInfo = listItemsInfo[itemCell.index];
        ItemsBean itemsData = new ItemsBean();
        itemsData.itemId = itemsInfo.id;
        itemsData.number = int.MaxValue;
        viewItemContainer.SetViewItemByData(UIViewItemContainer.ContainerType.God, itemsData, itemCell.index);
    }

    /// <summary>
    /// 处理时间改变
    /// </summary>
    public void HandleForTimeChange(int hour, int min)
    {
        GameTimeHandler.Instance.SetGameTime(hour, min);
    }
}