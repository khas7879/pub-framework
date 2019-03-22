using UnityEngine;
using System.Collections;
using System;
using CJC.Framework.Rule.Base;

public class RuleObject : IObjectBase
{
	[Space(10)]
	[Header("测试_数组")]
	public int[] testArr;

	public CRule RuleHost;

	public override bool XmlSerilize(ref XmlData data)
	{
		if (null == RuleHost)
			return false;

		if (!RuleHost.XmlSerilize(ref data))
			return false;

		data.SetAttribute(ERuleEditorKey.Position, ToolParser.StringParser(Position));
		return true;
	}

	public override bool XmlDeserilize(XmlData data)
	{
		Position = ToolParser.VectorParser(data.GetAttribute(ERuleEditorKey.Position));

		if (!RuleHost.XmlDeserilize(data))
			return false;

		return true;
	}

	public override void Print()
	{
		
	}

	public RuleObject() { RuleHost = new CRule(0);  }
	private void SortExecutersByDepth() { }
	public ExecuterObject TryGetExecuterAtPos(Vector2 position) { return null; }	// 先根据深度排序 再取
}
