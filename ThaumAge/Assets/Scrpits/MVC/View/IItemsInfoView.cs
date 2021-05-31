/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IItemsInfoView
{
	void GetItemsInfoSuccess<T>(T data, Action<T> action);

	void GetItemsInfoFail(string failMsg, Action action);
}