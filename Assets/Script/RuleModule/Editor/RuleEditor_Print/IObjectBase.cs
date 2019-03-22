using UnityEngine;
using System.Collections;
using System;
using CJC.Framework.IO;

public abstract class IObjectBase : ScriptableObject, IXmlDataSerilizer
{
	private long Depth;	// 简单的操作一次递加就好
	protected Vector2 Position;

	public abstract void Print();

	public abstract bool XmlSerilize(ref XmlData data);

	public abstract bool XmlDeserilize(XmlData data);


	public virtual void Update() { }

	public void SetPosition(Vector2 position) { Position = position; }
}

public class ERuleEditorKey
{
	public const string Position = "position";
	
}