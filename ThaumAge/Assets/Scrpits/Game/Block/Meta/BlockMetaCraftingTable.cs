using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaCraftingTable : BlockMetaBase
{
    public int magic;
    public int maxMagic;

    public TimeBean closeTime;

    public int GetTimeAddMagic(TimeBean curentTime)
    {
        int addMagic = (curentTime.second - closeTime.second) + (curentTime.minute- closeTime.minute) * 60 + (curentTime.hour - closeTime.hour) * 360;
        closeTime = curentTime;
        if (addMagic < 0)
            addMagic = 0;
        return addMagic;
    }

    public void AddMagic(int addMagic)
    {
        magic += addMagic;
        if (magic> maxMagic)
        {
            magic = maxMagic;
        }
        if (magic < 0)
        {
            magic = 0;
        }
    }
}