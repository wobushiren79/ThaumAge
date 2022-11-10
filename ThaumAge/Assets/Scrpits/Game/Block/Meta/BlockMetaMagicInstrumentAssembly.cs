using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaMagicInstrumentAssembly : BlockMetaBase
{
    public ItemsBean capData;
    public ItemsBean rodData;
    public ItemsBean wandData;

    public void ClearData()
    {
        capData = null;
        rodData = null;
        wandData = null;
    }
}