using UnityEditor;
using UnityEngine;

public partial class UIViewMagicItem : BaseUIView
{

    protected Item item;
    protected ItemsBean itemData;
    protected ItemsInfoBean itemsInfo;
    protected ItemMetaMagicCore itemMetaMagicCoreData;

    protected int indexMagic;

    public override void OnEnable()
    {
        base.OnEnable();
        EventHandler.Instance.RegisterEvent<int>(EventsInfo.UIViewShortcutsMagic_ChangeSelect, CallBackForChangeSelect);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        EventHandler.Instance.UnRegisterEvent<int>(EventsInfo.UIViewShortcutsMagic_ChangeSelect, CallBackForChangeSelect);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(ItemsBean itemData,int indexMagic)
    {
        this.indexMagic = indexMagic;
        this.itemData = itemData;
        //获取法术核心数据
        itemMetaMagicCoreData = itemData.GetMetaData<ItemMetaMagicCore>();
        item = ItemsHandler.Instance.manager.GetRegisterItem(itemData.itemId);
        itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        ui_Select.ShowObj(false);
        SetMagicIcon();
        SetButtonHint();
    }

    /// <summary>
    /// 设置魔法图标
    /// </summary>
    public void SetMagicIcon()
    {
        ElementalTypeEnum elementalType =  itemMetaMagicCoreData.GetElement();
        ElementalInfoBean elementalInfo = ElementalInfoCfg.GetItemData(elementalType);
        IconHandler.Instance.manager.GetUISpriteByName(elementalInfo.icon_key,(sprite)=> 
        {
            ColorUtility.TryParseHtmlString($"{elementalInfo.color}",out Color colorIcon);
            ui_Icon.sprite = sprite;
            ui_Icon.color = colorIcon;
        });
    }

    public void SetButtonHint()
    {
        ui_ButtonHint.text= $"F{indexMagic + 1}";
    }

    public void CallBackForChangeSelect(int indexSelect)
    {
        if (indexSelect == indexMagic)
        {
            ui_Select.ShowObj(true);
        }
        else
        {
            ui_Select.ShowObj(false);
        }
    }
}