using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


// 每一个数据单位
public abstract partial class IModelData
{
	public long UID;
	private void _OnDestroy()
	{
		OnDestroy();

		ClearRule();
		ClearTimer();
	}

	protected virtual void OnDestroy() { }
}