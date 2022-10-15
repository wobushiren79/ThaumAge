using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaLiquid : BlockMetaBase
{
    //体积 最高为8次满
    public int volume = 8;

    public int AddVolume(int addVolume)
    {
        volume += addVolume;
        if (volume < 0)
        {
            volume = 0;
        }
        else if (volume > 8)
        {
            volume = 8;
        }
        return volume;
    }
}