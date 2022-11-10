/*
* FileName: MagicInstrumentInfo 
* Author: AppleCoffee 
* CreateTime: 2022-11-10-18:16:13 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IMagicInstrumentInfoView
{
	void GetMagicInstrumentInfoSuccess<T>(T data, Action<T> action);

	void GetMagicInstrumentInfoFail(string failMsg, Action action);
}