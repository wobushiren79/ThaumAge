using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIBaseEntity : BaseMonoBehaviour
{
    //意图列表
    public List<AIBaseIntent> listIntent = new List<AIBaseIntent>();
    //当前意图
    public AIBaseIntent currentIntent;

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
            currentIntent.IntentActUpdate();
        }
    }

    public virtual void FixedUpdate()
    {
        if (currentIntent != null)
        {
            currentIntent.IntentActFixUpdate();
        }
    }
    
    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="listIntent"></param>
    public void SetIntent(List<AIBaseIntent> listIntent)
    {
        this.listIntent = listIntent;
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
    public void ChangeIntent(AIIntentEnum aiIntent)
    {
        if (CheckUtil.ListIsNull(listIntent))
        {
            LogUtil.LogWarning("转换AI意图" + aiIntent.ToString() + "失败，还没有初始化相关AI意图");
            return;
        }
        if (currentIntent != null)
        {
            currentIntent.IntentLeaving();
        }
        for (int i = 0; i < listIntent.Count; i++)
        {
            AIBaseIntent aiBaseIntent = listIntent[i];
            if (aiBaseIntent.aiIntent == aiIntent)
            {
                currentIntent = aiBaseIntent;
                currentIntent.IntentEntering();
                return;
            }
        }
        LogUtil.LogWarning("转换AI意图" + aiIntent.ToString() + "失败,没有相关AI意图");
    }

}