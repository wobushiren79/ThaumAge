﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtension
{

    /// <summary>
    /// 获取包含名字的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="includeSele"></param>
    /// <returns></returns>
    public static List<T> GetComponentsInChildrenContainsName<T>(this T selfComponent, string name, bool includeSele = false) where T : Component
    {
        List<T> listData = new List<T>();
        T[] cptList = selfComponent.GetComponentsInChildren<T>(includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            T item = cptList[i];
            if (item.name.Contains(name))
            {
                listData.Add(item);
            }
        }
        return listData;
    }

    /// <summary>
    /// 获取包含名字的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="includeSele"></param>
    /// <returns></returns>
    public static T GetComponentInChildrenContainsName<T>(this T selfComponent, string name, bool includeSele = false) where T : Component
    {
        T[] cptList = selfComponent.GetComponentsInChildren<T>(includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            T item = cptList[i];
            if (item.name.Contains(name))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取指定名字的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="includeSele"></param>
    /// <returns></returns>
    public static List<T> GetComponentsInChildren<T>(this T selfComponent, string name, bool includeSele = false) where T : Component
    {
        List<T> listData = new List<T>();
        T[] cptList = selfComponent.GetComponentsInChildren<T>(includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            T item = cptList[i];
            if (item.name.Equals(name))
            {
                listData.Add(item);
            }
        }
        return listData;
    }

    public static List<Component> GetComponentsInChildren<T>(this T selfComponent, string name, Type type,  bool includeSele = false) where T : Component
    {
        List<Component> listData = new List<Component>();
        Component[] cptList = selfComponent.GetComponentsInChildren(type , includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            Component item = cptList[i];
            if (item.name.Equals(name))
            {
                listData.Add(item);
            }
        }
        return listData;
    }

    public static Component GetComponentInChildren<T>(this T selfComponent, string name, Type type, bool includeSele = false) where T : Component
    {
        List<Component> listData = new List<Component>();
        Component[] cptList = selfComponent.GetComponentsInChildren(type, includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            Component item = cptList[i];
            if (item.name.Equals(name))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取指定名字的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="includeSele"></param>
    /// <returns></returns>
    public static T GetComponentInChildren<T>(this T selfComponent, string name, bool includeSele = false) where T : Component
    {
        T[] cptList = selfComponent.GetComponentsInChildren<T>(includeSele);
        for (int i = 0; i < cptList.Length; i++)
        {
            T item = cptList[i];
            if (item.name.Equals(name))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 添加组件-扩展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <returns></returns>
    public static T AddComponentEX<T>(this GameObject selfComponent) 
        where T : Component
    {
        T addComponet = selfComponent.GetComponent<T>();
        if (addComponet == null)
        {
            addComponet = selfComponent.AddComponent<T>();
        }
        return addComponet;
    }
    public static T AddComponentEX<T>(this Transform selfComponent)
    where T : Component
    {
        T addComponet = selfComponent.GetComponent<T>();
        if (addComponet == null)
        {
            addComponet = selfComponent.gameObject.AddComponent<T>();
        }
        return addComponet;
    }

    /// <summary>
    /// 获取子物体数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public static int GetChildCount<T>(this T selfComponent, bool isActive) where T : Component
    {
        int number = 0;
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            if (selfComponent.transform.GetChild(i).gameObject.activeSelf == isActive)
            {
                number++;
            }
        }
        return number;
    }

    /// <summary>
    /// 获取子物体数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <returns></returns>
    public static int GetChildCount<T>(this T selfComponent) where T : Component
    {
        return selfComponent.transform.childCount;
    }

    /// <summary>
    /// 删除包含名字的物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestoryChildContainsName<T>(this T selfComponent, string name, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (childTF.name.Contains(name))
            {
                if (mode == 1)
                {
                    GameObject.DestroyImmediate(childTF.gameObject);
                    i--;
                }
                else
                {
                    GameObject.Destroy(childTF.gameObject);
                }
            }
        }
        return selfComponent;
    }

    /// <summary>
    ///  删除包含名字的物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestoryChildContainsName<T>(this T selfComponent, string name, bool isActive, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (childTF.name.Contains(name))
            {
                if (childTF.gameObject.activeSelf == isActive)
                {
                    if (mode == 1)
                    {
                        GameObject.DestroyImmediate(childTF.gameObject);
                        i--;
                    }
                    else
                    {
                        GameObject.Destroy(childTF.gameObject);
                    }
                }
            }
        }
        return selfComponent;
    }

    /// <summary>
    /// 删除同名子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestoryChild<T>(this T selfComponent, string name, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (childTF.name.Equals(name))
            {
                if (mode == 1)
                {
                    GameObject.DestroyImmediate(childTF.gameObject);
                    i--;
                }
                else
                {
                    GameObject.Destroy(childTF.gameObject);
                }
            }
        }
        return selfComponent;
    }

    /// <summary>
    /// 删除同名子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="name"></param>
    /// <param name="isActive"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestoryChild<T>(this T selfComponent, string name, bool isActive, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (childTF.name.Equals(name))
            {
                if (childTF.gameObject.activeSelf == isActive)
                {
                    if (mode == 1)
                    {
                        GameObject.DestroyImmediate(childTF.gameObject);
                        i--;
                    }
                    else
                    {
                        GameObject.Destroy(childTF.gameObject);
                    }
                }
            }
        }
        return selfComponent;
    }

    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestroyAllChild<T>(this T selfComponent, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (mode == 1)
            {
                GameObject.DestroyImmediate(childTF.gameObject);
                i--;
            }
            else
            {
                GameObject.Destroy(childTF.gameObject);
            }
        }
        return selfComponent;
    }

    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="isActive"></param>
    /// <param name="mode">0:Destroy 1:DestroyImmediate</param>
    /// <returns></returns>
    public static T DestroyAllChild<T>(this T selfComponent, bool isActive, int mode = 0) where T : Component
    {
        for (int i = 0; i < selfComponent.transform.childCount; i++)
        {
            Transform childTF = selfComponent.transform.GetChild(i);
            if (childTF.gameObject.activeSelf == isActive)
            {
                if (mode == 1)
                {
                    GameObject.DestroyImmediate(childTF.gameObject);
                    i--;
                }
                else
                {
                    GameObject.Destroy(childTF.gameObject);
                }
            }
        }
        return selfComponent;
    }

    /// <summary>
    /// 展示OBJ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selfComponent"></param>
    /// <param name="isShow"></param>
    /// <returns></returns>
    public static T ShowObj<T>(this T selfComponent,bool isShow = true) where T : Component
    {
        selfComponent.gameObject.SetActive(isShow);
        return selfComponent;
    }

    /// <summary>
    /// 展示OBJ
    /// </summary>
    /// <param name="selfObj"></param>
    /// <param name="isShow"></param>
    /// <returns></returns>
    public static GameObject ShowObj(this GameObject selfObj, bool isShow = true)
    {
        selfObj.SetActive(isShow);
        return selfObj;
    }

}