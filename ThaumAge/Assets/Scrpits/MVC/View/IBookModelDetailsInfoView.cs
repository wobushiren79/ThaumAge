/*
* FileName: BookModelDetailsInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:33:00 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBookModelDetailsInfoView
{
	void GetBookModelDetailsInfoSuccess<T>(T data, Action<T> action);

	void GetBookModelDetailsInfoFail(string failMsg, Action action);
}