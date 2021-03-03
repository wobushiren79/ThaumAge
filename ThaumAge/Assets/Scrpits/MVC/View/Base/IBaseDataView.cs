using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IBaseDataView 
{
     void GetAllBaseDataSuccess(List<BaseDataBean> listData);

     void GetAllBaseDataFail(string failMsg);

}