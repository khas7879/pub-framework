using UnityEngine;
using System.Collections;
using System;

public class RuleObject : IObjectBase
{
	public override void Export()
	{
		throw new NotImplementedException();
	}

	public override void Import()
	{
		throw new NotImplementedException();
	}

	public override void Print()
	{
		throw new NotImplementedException();
	}

	private void SortExecutersByDepth() { }
	public ExecuterObject TryGetExecuterAtPos(Vector2 position) { return null; }	// 先根据深度排序 再取
}
