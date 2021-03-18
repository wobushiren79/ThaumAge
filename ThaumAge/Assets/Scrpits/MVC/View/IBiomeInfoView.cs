/*
* FileName: BiomeInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-18-17:53:13 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBiomeInfoView
{
	void GetBiomeInfoSuccess<T>(T data, Action<T> action);

	void GetBiomeInfoFail(string failMsg, Action action);
}