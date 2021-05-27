using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Test : BaseMonoBehaviour
{
    private void Start()
    {
        UIHandler.Instance.manager.OpenUI<UIGameMain>(UIEnum.GameMain);
    }
}
