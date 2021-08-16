/*
* FileName: Sheet1 
* Author: AppleCoffee 
* CreateTime: 2021-05-26-18:49:53 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface ISheet1View
{
	void GetSheet1Success<T>(T data, Action<T> action);

	void GetSheet1Fail(string failMsg, Action action);
}