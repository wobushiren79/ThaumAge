/*
* FileName: ElementalInfo 
* Author: AppleCoffee 
* CreateTime: 2022-10-11-21:25:04 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IElementalInfoView
{
	void GetElementalInfoSuccess<T>(T data, Action<T> action);

	void GetElementalInfoFail(string failMsg, Action action);
}