/*
* FileName: ItemsSynthesis 
* Author: AppleCoffee 
* CreateTime: 2021-12-28-17:06:25 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IItemsSynthesisView
{
	void GetItemsSynthesisSuccess<T>(T data, Action<T> action);

	void GetItemsSynthesisFail(string failMsg, Action action);
}