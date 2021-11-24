/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IChunkSaveView
{
	void GetChunkSaveSuccess<T>(T data, Action<T> action);

	void GetChunkSaveFail(string failMsg, Action action);
}