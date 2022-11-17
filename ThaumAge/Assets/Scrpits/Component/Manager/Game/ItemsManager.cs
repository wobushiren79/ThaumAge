using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : BaseManager,IItemsInfoView
{
    protected ItemsInfoController controllerForItems;

    //道具信息列表
    protected Dictionary<long, ItemsInfoBean> dicItemsInfo = new();
    //道具信息列表
    protected List<ItemsInfoBean> listItemsInfo = new();

    //注册道具列表
    protected Item[] arrayItemRegister = new Item[EnumExtension.GetEnumMaxIndex<ItemsTypeEnum>() + 1];
    protected Dictionary<long, Item> dicItemRegisterForId = new Dictionary<long, Item>();

    //道具模型列表
    protected Dictionary<long, GameObject> dicItemsObj = new();
    //道具备用模型列表(用于衣服或者裤子的延长部分)
    protected Dictionary<long, IList<GameObject>> dicItemsRemarkObj = new();
    //道具模型贴图
    protected Dictionary<long, Texture> dicItemsTex = new();

    //路径-道具丢弃模型
    public static string PathForItemDrop = "Assets/Prefabs/Game/Item/ItemDrop.prefab";
    //路径-道具发射模型
    public static string PathForItemLaunch = "Assets/Prefabs/Game/Item/ItemLaunch.prefab";
    //路径-装备模型
    public static string PathEquipModel = "Assets/Prefabs/Model/Character/Equip";

    protected void Awake()
    {
        controllerForItems = new ItemsInfoController(this, this);
        controllerForItems.GetAllItemsInfoData(InitItemsInfo);
    }

    /// <summary>
    /// 初始化道具信息
    /// </summary>
    /// <param name="listItemsInfo"></param>
    public void InitItemsInfo(List<ItemsInfoBean> listItemsInfo)
    {
        if (listItemsInfo == null)
            return;
        for (int i = 0; i < listItemsInfo.Count; i++)
        {
            var itemData = listItemsInfo[i];
        }
        this.listItemsInfo = listItemsInfo
            .OrderBy(data =>
            {
                return data.id;
            })
            .ToList();
        InitData(dicItemsInfo, listItemsInfo);
        RegisterItem();
    }

    /// <summary>
    /// 获取所有物体信息
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> GetAllItemsInfo()
    {
        return listItemsInfo;
    }

    /// <summary>
    /// 根据ID获取物品数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfoById(long id)
    {
        return GetDataById(id, dicItemsInfo);
    }

    /// <summary>
    /// 通过方块ID获取道具ID
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfoByBlockId(int blockId)
    {
        for (int i = 0; i < listItemsInfo.Count; i++)
        {
            ItemsInfoBean itemsInfo = listItemsInfo[i];
            if (itemsInfo.type_id == blockId)
            {
                return itemsInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 通过方块类型获取道具ID
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public ItemsInfoBean GetItemsInfoByBlockType(BlockTypeEnum blockType)
    {
        return GetItemsInfoByBlockId((int)blockType);
    }

    /// <summary>
    /// 根据ID获取物品数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> GetItemsInfoById(List<long> ids)
    {
        List<ItemsInfoBean> listData = new List<ItemsInfoBean>();
        for (int i = 0; i < ids.Count; i++)
        {
            ItemsInfoBean itemData = GetItemsInfoById(ids[i]);
            listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 通过ID获取道具图标
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void GetItemsIconById(long id, Action<Sprite> callBack)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        IconHandler.Instance.manager.GetItemsSpriteByName(itemsInfo.icon_key, callBack);
    }

    /// <summary>
    /// 获取道具掉落的模型
    /// </summary>
    /// <param name="callBack"></param>
    public void GetItemsDropObj(Action<GameObject> callBack)
    {
        //添加道具掉落模型
        GetModelForAddressables(dicItemsObj, -1, PathForItemDrop, callBack);
    }

    /// <summary>
    /// 获取道具发射模型
    /// </summary>
    /// <param name="callBack"></param>
    public void GetItemsLaunchObj(Action<GameObject> callBack)
    {
        //添加道具掉落模型
        GetModelForAddressables(dicItemsObj, -2, PathForItemLaunch, callBack);
    }

    /// <summary>
    /// 获取物品模型(衣服 武器之类的)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callBack"></param>
    public void GetItemsObjById(long id, Action<GameObject> callBack, Action<IList<GameObject>> callBackForRemark)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        if (itemsInfo == null)
        {
            if (dicItemsObj.TryGetValue(id, out GameObject value))
            {
                callBack.Invoke(value);
            }
            if (dicItemsRemarkObj.TryGetValue(id, out IList<GameObject> valueRemark))
            {
                callBackForRemark.Invoke(valueRemark);
            }
        }
        else
        {
            GetModelForAddressables(dicItemsObj, id, $"{PathEquipModel}/{itemsInfo.model_name}.prefab", callBack);
            List<string> listModelRemarkName = itemsInfo.GetModelRemarkName(PathEquipModel);
            GetModelsForAddressables(dicItemsRemarkObj, id, listModelRemarkName, callBackForRemark);
        }
    }

    /// <summary>
    /// 获取道具模型贴图
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callBack"></param>
    public void GetItemsTexById(long id, Action<Texture> callBack)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        if (itemsInfo == null)
        {
            if (dicItemsTex.TryGetValue(id, out Texture value))
            {
                callBack?.Invoke(value);
            }
        }
        else
        {
            GetModelForAddressables(dicItemsTex, id, itemsInfo.tex_name, callBack);
        }
    }

    /// <summary>
    /// 获取注册物品类
    /// </summary>
    public Item GetRegisterItem(ItemsTypeEnum itemsType)
    {
        return arrayItemRegister[(int)itemsType];
    }

    /// <summary>
    /// 获取注册物品类
    /// </summary>
    public Item GetRegisterItem(long itemId, ItemsTypeEnum itemsType = ItemsTypeEnum.None)
    {
        if (dicItemRegisterForId.TryGetValue(itemId, out Item item))
        {
            return item;
        }
        if (itemsType != ItemsTypeEnum.None)
        {
            return GetRegisterItem(itemsType);
        }
        return null;
    }

    /// <summary>
    /// 注册所有方块
    /// </summary>
    public void RegisterItem()
    {
        //注册所有有类的道具
        dicItemRegisterForId.Clear();
        for (int i = 0; i < listItemsInfo.Count; i++)
        {
            var itemInfo = listItemsInfo[i];
            //通过反射获取类
            Item item = ReflexUtil.CreateInstance<Item>($"ItemClass{itemInfo.link_class}");
            if (item != null)
            {
                RegisterItem(itemInfo.id, item);
            }
        }
        //注册所有类型道具
        List<ItemsTypeEnum> listItemsType = EnumExtension.GetEnumValue<ItemsTypeEnum>();
        for (int i = 0; i < listItemsType.Count; i++)
        {
            ItemsTypeEnum itemsType = listItemsType[i];
            string itemsTypeName = EnumExtension.GetEnumName(itemsType);
            //通过反射获取类
            Item item = ReflexUtil.CreateInstance<Item>($"ItemType{itemsTypeName}");
            if (item == null)
            {
                item = new Item();
            }
            RegisterItem(itemsType, item);
        }
    }

    public void RegisterItem(ItemsTypeEnum itemsType, Item item)
    {
        arrayItemRegister[(int)itemsType] = item;
    }
    public void RegisterItem(int itemsId, Item item)
    {
        dicItemRegisterForId.Add(itemsId, item);
    }

    #region 数据回调
    public void GetItemsInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetItemsInfoFail(string failMsg, Action action)
    {

    }
    #endregion
}