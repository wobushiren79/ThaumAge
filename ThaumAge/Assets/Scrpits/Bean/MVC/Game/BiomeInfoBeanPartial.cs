using System;
using System.Collections.Generic;
public partial class BiomeInfoBean
{
    public struct BiomeInfoCreatureCreateData
    {
        //����Id
        public int creatureId;
        //���ﴴ���ķ�Χ
        public int createRange;
        //���ﴴ����Χ�ڵ������������
        public int createRnageMaxNum;
    }

    protected List<BiomeInfoCreatureCreateData> creatureCreateData;

    public List<BiomeInfoCreatureCreateData> GetCreatureCreateData()
    {
        if (creatureCreateData == null)
        {
            creatureCreateData = new List<BiomeInfoCreatureCreateData>();
            if (!creatureData.IsNull())
            {
                string[] creatureDataStr = creatureData.Split("&");
                for (int i = 0; i < creatureDataStr.Length; i++)
                {
                    string itemCreatureDataStr = creatureDataStr[i];
                    string[] itemCreatureData = itemCreatureDataStr.Split('_');
                    BiomeInfoCreatureCreateData biomeInfoCreatureCreateData = new BiomeInfoCreatureCreateData();
                    biomeInfoCreatureCreateData.creatureId = int.Parse(itemCreatureData[0]);
                    biomeInfoCreatureCreateData.createRange = int.Parse(itemCreatureData[1]);
                    biomeInfoCreatureCreateData.createRnageMaxNum = int.Parse(itemCreatureData[2]);
                    creatureCreateData.Add(biomeInfoCreatureCreateData);
                }
            }
        }
        return creatureCreateData;
    }
}
public partial class BiomeInfoCfg
{

}
