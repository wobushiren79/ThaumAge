/*
* FileName: CreatureInfo 
* Author: AppleCoffee 
* CreateTime: 2021-12-07-10:59:41 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface ICreatureInfoView
{
	void GetCreatureInfoSuccess<T>(T data, Action<T> action);

	void GetCreatureInfoFail(string failMsg, Action action);
}