/*
* FileName: BiomeSave 
* Author: AppleCoffee 
* CreateTime: 2022-11-01-14:57:35 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IBiomeSaveView
{
	void GetBiomeSaveSuccess<T>(T data, Action<T> action);

	void GetBiomeSaveFail(string failMsg, Action action);
}