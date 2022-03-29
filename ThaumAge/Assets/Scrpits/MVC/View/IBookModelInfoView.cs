/*
* FileName: BookModelInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:32:49 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBookModelInfoView
{
	void GetBookModelInfoSuccess<T>(T data, Action<T> action);

	void GetBookModelInfoFail(string failMsg, Action action);
}