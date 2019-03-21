using UnityEngine;
using System.Collections;
using System;

public class ExecuterObject : IObjectBase
{
	[Header("测试_字符串")]
	public string testStr = "Test String";
	[Space(10)]
	[Header("测试_数组")]
	public int[] testArr;

	public override void Print()
	{
		throw new NotImplementedException();
	}

	public override void Export()
	{
		throw new NotImplementedException();
	}

	public override void Import()
	{
		throw new NotImplementedException();
	}
}
