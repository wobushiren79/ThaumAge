using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        WorldCreateHandler.Instance.CreateChunk(30,30);
    }


}
