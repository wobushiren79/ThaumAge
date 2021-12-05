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
    protected Item[] arrayItemRegister = new Item[EnumExtension.GetEnumMaxIndex<ItemsTypeEnum>() + 1];

    //道具模型列表
    protected Dictionary<long, GameObject> dicItemsObj = new Dictionary<long, GameObject>();
    //道具模型贴图
    protected Dictionary<long, Texture> dicItemsTex = new Dictionary<long, Texture>();

    //路径-道具丢弃模型
    public static string pathForItemCptDrop = "Assets/Prefabs/Game/ItemCptDrop.prefab";
    protected void Awake()
    {
        controllerForItems = new ItemsInfoController(this, this);
        controllerForItems.GetAllItemsInfoData(InitItemsInfo);
        RegisterItem();
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
    /// 获取物品模型(衣服 武器之类的)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="callBack"></param>
    public void GetItemsObjById(long id, Action<GameObject> callBack)
    {
        ItemsInfoBean itemsInfo = GetItemsInfoById(id);
        if (itemsInfo == null)
        {
            if (dicItemsObj.TryGetValue(id, out GameObject value))
            {
                callBack?.Invoke(value);
            }
            else
            {
                //如果找不到该模型
                if (id == -1)
                {
                    //添加道具掉落模型
                    GetModelForAddressables(dicItemsObj, -1, pathForItemCptDrop, callBack);
                }
            }
        }
        else
        {
            GetModelForAddressables(dicItemsObj, id, itemsInfo.model_name, callBack);
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
        List<ItemsTypeEnum> listItemsType = EnumExtension.GetEnumValue<ItemsTypeEnum>();
        for (int i = 0; i < listItemsType.Count; i++)
        {
            ItemsTypeEnum itemsType = listItemsType[i];
            string itemsTypeName = EnumExtension.GetEnumName(itemsType);
            //通过反射获取类
            Item item = ReflexUtil.CreateInstance<Item>($"Item{itemsTypeName}");
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