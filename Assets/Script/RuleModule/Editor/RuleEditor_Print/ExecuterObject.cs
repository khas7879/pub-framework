using UnityEngine;
using System.Collections;
using System;
using CJC.Framework.Rule.Base;

public class ExecuterObject : IObjectBase
{
	[Header("测试_字符串")]
	public string testStr = "Test String";
	[Space(10)]
	[Header("测试_数组")]
	public int[] testArr;

	private RuleExcuter ExecuterHost;

	public override void Print()
	{
		throw new NotImplementedException();
	}

	public override bool XmlSerilize(ref XmlData data)
	{
		if (null == ExecuterHost)
			return false;

		if (!ExecuterHost.XmlSerilize(ref data))
			return false;

		data.SetAttribute(ERuleEditorKey.Position, ToolParser.StringParser(Position));
		return true;
	}

	public override bool XmlDeserilize(XmlData data)
	{
		throw new NotImplementedException();
	}
}
