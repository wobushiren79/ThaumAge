using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemsManager : BaseManager, IItemsInfoView
{
    protected ItemsInfoController controllerForItems;
    protected Dictionary<long, ItemsInfoBean> dicItemsInfo = new Dictionary<long, ItemsInfoBean>();
    protected List<ItemsInfoBean> listItemsInfo = new List<ItemsInfoBean>();

    //注册道具列表
    protected Item[] arrayItemRegister = new Item[EnumUtil.GetEnumMaxIndex<ItemsTypeEnum>() + 1];

    //道具模型列表
    protected Dictionary<long, GameObject> dicItemsObj = new Dictionary<long, GameObject>();
    //道具模型贴图
    protected Dictionary<long, Texture> dicItemsTex = new Dictionary<long, Texture>();

    protected void Awake()
    {
        controllerForItems = new ItemsInfoController(this, this);
        controllerForItems.GetAllItemsInfoData(InitItemsInfo);
        RegisterItem();
    }

    public void InitItemsInfo(List<ItemsInfoBean> listItemsInfo)
    {
        if (listItemsInfo == null)
            return;
        this.listItemsInfo = listItemsInfo
            .OrderBy(data =>
            {
                return data.id;
            })
            .ToList();
        dicItemsInfo = TypeConversionUtil.ListToMap(listItemsInfo);
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
        if (dicItemsInfo.TryGetValue(id, out ItemsInfoBean itemsInfo))
        {
            return itemsInfo;
        }
        return null;
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
    /// 获取物品模型
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callBack"></param>
    public void GetItemsObjById(long id, Action<GameObject> callBack)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        GetModelForAddressables(dicItemsObj, id, itemsInfo.model_name, callBack);
    }

    /// <summary>
    /// 获取道具模型贴图
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callBack"></param>
    public void GetItemsTexById(long id, Action<Texture> callBack)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        GetModelForAddressables(dicItemsTex, id, itemsInfo.tex_name, callBack);
    }

    /// <summary>
    /// 获取注册物品类
    /// </summary>
    /// <param name="itemsType"></param>
    /// <returns></returns>
    public Item GetRegisterItem(ItemsTypeEnum itemsType)
    {
        return arrayItemRegister[(int)itemsType];
    }


    /// <summary>
    /// 注册所有方块
    /// </summary>
    public void RegisterItem()
    {
        List<ItemsTypeEnum> listItemsType = EnumUtil.GetEnumValue<ItemsTypeEnum>();
        for (int i = 0; i < listItemsType.Count; i++)
        {
            ItemsTypeEnum itemsType = listItemsType[i];
            string itemsTypeName = EnumUtil.GetEnumName(itemsType);
            //通过反射获取类
            Item item = ReflexUtil.CreateInstance<Item>("Item" + itemsTypeName);
            RegisterItem(itemsType, item);
        }
    }

    public void RegisterItem(ItemsTypeEnum itemsType, Item item)
    {
        arrayItemRegister[(int)itemsType] = item;
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