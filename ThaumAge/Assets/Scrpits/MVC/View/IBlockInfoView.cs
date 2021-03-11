/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBlockInfoView
{
	void GetBlockInfoSuccess<T>(T data, Action<T> action);

	void GetBlockInfoFail(string failMsg, Action action);
}