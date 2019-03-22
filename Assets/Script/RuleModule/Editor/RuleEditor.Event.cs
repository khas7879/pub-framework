using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public partial class RuleEditor
{
	private IObjectBase HandleObject;   // 判断鼠标持有的对象

	private void UpdateEvent()
	{
		RuleMouseEventBase ruleEvent = GetUpdateEvent(Event.current);

		if (null == ruleEvent) return;

		if(ruleEvent is RuleMouseDownEvent)
		{
			RuleMouseDownEvent e = ruleEvent as RuleMouseDownEvent;

			HandleObject = e.HandleTarget;
		}
		else if(ruleEvent is RuleMouseDragEvent)
		{
			RuleMouseDragEvent e = ruleEvent as RuleMouseDragEvent;

			if (null != HandleObject)

				HandleObject.SetPosition(e.CurrentPos);
		}
		else if(ruleEvent is RuleMouseUpEvent)
		{
			RuleMouseUpEvent e = ruleEvent as RuleMouseUpEvent;

			HandleObject = e.HandleTarget;
		}
	}

	#region 封装坑爹的event事件
	private IObjectBase ___target;
	private Vector2 ___mLastPosition;   // 用于记录上一帧的鼠标位置
	private Vector2 ___mStartClickPosition;    // 记录鼠标单击下的时候 的鼠标位置
	private bool ___isMouseDown;   // 判断鼠标是否是落下状态

	public RuleMouseEventBase GetUpdateEvent(Event e)
	{
		if (null == SelectedRuleObject)
			return null;

		if (null == e)
			return null;

		if (e.button == 1 && e.type == EventType.ContextClick)
		{
			var pTarget = SelectedRuleObject.TryGetExecuterAtPos(e.mousePosition);
			if (null == pTarget)
				return null;

			RuleContextEvent ruleContextEvent = new RuleContextEvent(e.mousePosition, pTarget);
			return ruleContextEvent;
		}

		if (e.button != 0)
			return null;

		if(mouseOverWindow != this)
		{
			RuleMouseEventBase eventClear = CheckOutWindow();
			___isMouseDown = false;
			___target = null;
			return eventClear;
		}

		switch(e.type)
		{
			case EventType.mouseDown:
				{
					___isMouseDown = true;
					___mStartClickPosition = e.mousePosition;
					___mLastPosition = e.mousePosition;

					___target = SelectedRuleObject.TryGetExecuterAtPos(e.mousePosition);
					if (null == ___target)
					{
						return null;
					}
					RuleMouseDownEvent downEvent = new RuleMouseDownEvent(e.mousePosition, ___target);
					return downEvent;
				}
			case EventType.mouseUp:
				{
					___target = SelectedRuleObject.TryGetExecuterAtPos(e.mousePosition);
					RuleMouseUpEvent upEvent = new RuleMouseUpEvent(___mStartClickPosition, e.mousePosition, ___target);
					___isMouseDown = false;
					return upEvent;
				}
			case EventType.mouseDrag:
				{
					if(___isMouseDown && ___target != null)
					{
						RuleMouseDragEvent dragEvent = new RuleMouseDragEvent(___mLastPosition, e.mousePosition, ___target);
						___mLastPosition = e.mousePosition;
						return dragEvent;
					}
					return null;
				}
		}
		return null;
	}

	private RuleMouseEventBase CheckOutWindow()
	{
		if (!___isMouseDown)
			return null;

		if (null == ___target)
			return null;

		RuleOutWindowEvent outEvent = new RuleOutWindowEvent(___mStartClickPosition, ___mLastPosition, ___target);
		return outEvent;
	}

	public abstract class RuleMouseEventBase { }

	public class RuleMouseUpEvent : RuleMouseEventBase
	{
		public readonly Vector2 DownPos;
		public readonly Vector2 UpPos;
		public readonly IObjectBase HandleTarget;

		public RuleMouseUpEvent(Vector2 downPos, Vector2 upPos, IObjectBase target)
		{
			DownPos = downPos;
			UpPos = upPos;
			HandleTarget = target;
		}
	}

	public class RuleMouseDownEvent : RuleMouseEventBase
	{
		public readonly Vector2 DownPos;
		public readonly IObjectBase HandleTarget;

		public RuleMouseDownEvent(Vector2 downPos, IObjectBase target)
		{
			DownPos = downPos;
			HandleTarget = target;
		}
	}

	public class RuleMouseClickEvent : RuleMouseEventBase { }//有需求再做，判断点下和起来是同一个对象

	public class RuleMouseDragEvent : RuleMouseEventBase
	{
		public readonly Vector2 LastPos;
		public readonly Vector2 CurrentPos;
		public readonly IObjectBase HandleTarget;

		public RuleMouseDragEvent(Vector2 lastPos, Vector2 curPos, IObjectBase target)
		{
			LastPos = lastPos;
			CurrentPos = curPos;
			HandleTarget = target;
		}
	}

	public class RuleOutWindowEvent : RuleMouseEventBase
	{
		public readonly Vector2 DownPos;
		public readonly Vector2 OutWindowPos;
		public readonly IObjectBase HandleTarget;

		public RuleOutWindowEvent(Vector2 downPos, Vector2 outWindowPos, IObjectBase target)
		{
			DownPos = downPos;
			OutWindowPos = outWindowPos;
			HandleTarget = target;
		}
	}

	public class RuleContextEvent: RuleMouseEventBase
	{
		public readonly Vector2 Position;
		public readonly IObjectBase HandleTarget;

		public RuleContextEvent(Vector2 position, IObjectBase target)
		{
			Position = position;
			HandleTarget = target;
		}
	}
	#endregion
}