using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : BaseManager,
    IItemsInfoView, IItemsSynthesisView
{
    protected ItemsInfoController controllerForItems;
    protected ItemsSynthesisController controllerForSynthesis;

    //道具信息列表
    protected Dictionary<long, ItemsInfoBean> dicItemsInfo = new();
    //道具合成列表
    protected Dictionary<long, ItemsSynthesisBean> dicItemsSynthesis = new();
    //道具信息列表
    protected List<ItemsInfoBean> listItemsInfo = new();

    //注册道具列表
    protected Item[] arrayItemRegister = new Item[EnumExtension.GetEnumMaxIndex<ItemsTypeEnum>() + 1];
    protected Dictionary<int, Item> dicItemRegisterForId = new Dictionary<int, Item>();
    //道具模型列表
    protected Dictionary<long, GameObject> dicItemsObj = new();
    //道具模型贴图
    protected Dictionary<long, Texture> dicItemsTex = new();

    //路径-道具丢弃模型
    public static string pathForItemDrop = "Assets/Prefabs/Game/ItemDrop.prefab";
    protected void Awake()
    {
        controllerForItems = new ItemsInfoController(this, this);
        controllerForItems.GetAllItemsInfoData(InitItemsInfo);

        controllerForSynthesis = new ItemsSynthesisController(this, this);
        controllerForSynthesis.GetAllItemsSynthesisData(InitItemsSynthesis);
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
    /// 初始化道具合成数据
    /// </summary>
    /// <param name="listItemsSynthesis"></param>
    public void InitItemsSynthesis(List<ItemsSynthesisBean> listItemsSynthesis)
    {
        InitData(dicItemsSynthesis, listItemsSynthesis);
    }

    /// <summary>
    /// 获取道具合成数据
    /// </summary>
    /// <param name="synthesisId"></param>
    /// <returns></returns>
    public ItemsSynthesisBean GetItemsSynthesis(long synthesisId)
    {
        return GetDataById(synthesisId, dicItemsSynthesis);
    }

    /// <summary>
    /// 通过类型获取道具合成数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemsSynthesisBean> GetItemsSynthesisByType(ItemsSynthesisTypeEnum[] types)
    {
        List<ItemsSynthesisBean> listData = new List<ItemsSynthesisBean>();
        foreach (var itemData in dicItemsSynthesis)
        {
            ItemsSynthesisBean itemValue = itemData.Value;
            if (itemValue.CheckSynthesisType(types))
            {
                listData.Add(itemValue);
            }
        }
        return listData;
    }
    public List<ItemsSynthesisBean> GetItemsSynthesisByType(ItemsSynthesisTypeEnum type)
    {
        ItemsSynthesisTypeEnum[] listType;
        if (type == ItemsSynthesisTypeEnum.Self)
        {
            listType = new ItemsSynthesisTypeEnum[] { type };
        }
        else
        {
            listType = new ItemsSynthesisTypeEnum[] { ItemsSynthesisTypeEnum.Self, type };
        }
        return GetItemsSynthesisByType(listType);
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
                    GetModelForAddressables(dicItemsObj, -1, pathForItemDrop, callBack);
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
    public Item GetRegisterItem(ItemsTypeEnum itemsType)
    {
        return arrayItemRegister[(int)itemsType];
    }

    /// <summary>
    /// 获取注册物品类
    /// </summary>
    public Item GetRegisterItem(int itemId, ItemsTypeEnum itemsType = ItemsTypeEnum.None)
    {
        if (dicItemRegisterForId.TryGetValue(itemId, out Item item))
        {
            return item;
        }
        if(itemsType != ItemsTypeEnum.None)
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
            Item item = ReflexUtil.CreateInstance<Item>($"{itemInfo.link_class}");
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

    public void GetItemsSynthesisSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetItemsSynthesisFail(string failMsg, Action action)
    {

    }
    #endregion
}