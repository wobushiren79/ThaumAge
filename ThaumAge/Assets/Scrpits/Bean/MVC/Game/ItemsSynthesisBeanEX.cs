using UnityEditor;
using UnityEngine;

public partial class ItemsSynthesisBean
{
    /// <summary>
    /// 检测合成类型 
    /// </summary>
    /// <param name="types"></param>
    public bool CheckSynthesisType(int[] types)
    {
        if (type_synthesis.IsNull())
            return true;
        int[] currentTypes = StringUtil.SplitBySubstringForArrayInt(type_synthesis, ',');       
        for (int i = 0; i < types.Length; i++)
        {
            int itemType = types[i];
            for (int f = 0; f < currentTypes.Length; f++)
            {
                if (itemType == currentTypes[f])
                    return true;
            }
        }
        return false;
    }
}