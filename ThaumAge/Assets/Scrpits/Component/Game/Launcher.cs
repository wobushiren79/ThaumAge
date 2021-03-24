using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        WorldCreateHandler.Instance.CreateChunkForRange(1, Vector3Int.zero, 1);

        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

}
