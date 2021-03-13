using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{

    void Start()
    {
        WorldCreateHandler.Instance.CreateChunk(1111, Vector3Int.zero, 16, 256, 50);
        WorldCreateHandler.Instance.CreateChunk(1111, Vector3Int.zero+ new Vector3Int(16,0,0), 16, 256, 50);
    }


}
