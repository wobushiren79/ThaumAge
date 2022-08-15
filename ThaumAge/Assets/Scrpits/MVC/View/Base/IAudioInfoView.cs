/*
* FileName: AudioInfo 
* Author: AppleCoffee 
* CreateTime: 2022-08-15-18:21:16 
*/

using UnityEngine;
using System;
using System.Collections.Generic;

public interface IAudioInfoView
{
	void GetAudioInfoSuccess<T>(T data, Action<T> action);

	void GetAudioInfoFail(string failMsg, Action action);
}