using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIBaseEntity : BaseMonoBehaviour
{
    //意图列表
    public List<AIBaseIntent> listIntent = new List<AIBaseIntent>();
    //当前意图
    public AIBaseIntent currentIntent;

    //意图池
    public static Dictionary<string, AIBaseIntent> dicIntentPool = new Dictionary<string, AIBaseIntent>();

    public virtual void Awake()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        if (currentIntent != null)
        {
            currentIntent.IntentUpdate(this);
        }
    }

    public virtual void FixedUpdate()
    {
        if (currentIntent != null)
        {
            currentIntent.IntentFixUpdate(this);
        }
    }

    /// <summary>
    /// 初始化数据(每一个子类都必须重写该方法 并且指定枚举)
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public virtual void InitData<E>() where E : Enum
    {
        InitIntent<E>();
    }

    /// <summary>
    /// 初始化意图
    /// </summary>
    /// <typeparam name="I"></typeparam>
    public virtual void InitIntent<E>()  where E : Enum
    {
        List<E> listIntent = EnumExtension.GetEnumValue<E>();
        for (int i = 0; i < listIntent.Count; i++)
        {
            E itemIntent = listIntent[i];
            string intentName = itemIntent.GetEnumName();
            string classNameTitle = itemIntent.GetType().Name.Replace("Enum", "");
            string className = $"{classNameTitle}{intentName}";
            //首先获取类池里面是否有这个意图
            if (!dicIntentPool.TryGetValue(className,out AIBaseIntent intentClass))
            {
                intentClass = ReflexUtil.CreateInstance<AIBaseIntent>(className);
            }
            if (intentClass != null)
            {
                intentClass.InitData(itemIntent,this);
                AddIntent(intentClass);
            }
        }
    }

    /// <summary>
    /// 增加意图
    /// </summary>
    /// <param name="intent"></param>
    public void AddIntent(AIBaseIntent intent)
    {
        listIntent.Add(intent);
    }

    /// <summary>
    /// 移除意图
    /// </summary>
    /// <param name="intent"></param>
    public void RemoveIntent(AIBaseIntent intent)
    {
        if (listIntent.Contains(intent))
            listIntent.Remove(intent);
    }

    /// <summary>
    /// 改变意图
    /// </summary>
    /// <param name="aiIntent"></param>
    public void ChangeIntent<E>(E aiIntent) where E : System.Enum
    {
        if (listIntent.IsNull())
        {
            LogUtil.LogWarning("转换AI意图" + aiIntent.ToString() + "失败，还没有初始化相关AI意图");
            return;
        }
        if (currentIntent != null)
        {
            currentIntent.IntentLeaving(this);
        }
        for (int i = 0; i < listIntent.Count; i++)
        {
            AIBaseIntent aiBaseIntent = listIntent[i];
            if (Equals(aiBaseIntent.aiIntent, aiIntent))
            {
                currentIntent = aiBaseIntent;
                currentIntent.IntentEntering(this);
                return;
            }
        }
        LogUtil.LogWarning("转换AI意图" + aiIntent.ToString() + "失败,没有相关AI意图");
    }

}