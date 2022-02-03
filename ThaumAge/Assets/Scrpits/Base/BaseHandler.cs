﻿using UnityEngine;
using UnityEditor;

public class BaseHandler<T,M> : BaseSingletonMonoBehaviour<T> 
    where M : BaseManager
    where T : BaseMonoBehaviour
{
    private M mManager;
    public virtual void Awake()
    {

    }

    public M manager
    {
        get
        {
            if (mManager == null)
            {
                mManager = gameObject.AddComponentEX<M>();
            }
            return mManager;
        }
    }
}