using UnityEngine;
using System.Collections;

public abstract class IObjectBase : ScriptableObject
{
	private long Depth;	// 简单的操作一次递加就好
	private Vector2 Position;

	public abstract void Print();

	public abstract void Export();

	public abstract void Import();

	public void SetPosition(Vector2 position) { Position = position; }
}
