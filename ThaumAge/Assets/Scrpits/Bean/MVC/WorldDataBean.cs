/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using System;
using System.Collections.Generic;

[Serializable]
public class WorldDataBean : BaseBean
{
    public int workdType = 0;
    public string userId;

    public List<ChunkBean> listChunkData = new List<ChunkBean>();

    public WorldTypeEnum GetWorkType()
    {
        return (WorldTypeEnum)workdType;
    }

}