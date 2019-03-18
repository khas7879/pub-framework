using System;
using System.Collections.Generic;

using CJC.Framework.Event.Base;

// 可以做个对象池
namespace CJC.Framework.Timer
{
	public class CTimerManager
	{
		private static CTimerManager instance;

		public static CTimerManager Instance
		{
			get
			{
				if (null == instance)
					instance = new CTimerManager();
				return instance;
			}
		}

		private List<EventTimer> mTimerList;
		private CTimerManager() { mTimerList = new List<EventTimer>(); }

		public void Refresh(float deltaTime)
		{
			for(int idx = mTimerList.Count; idx > 0; )
			{
				var timer = mTimerList[--idx];
				timer.Refresh(deltaTime);
			}
		}

		private void OnTimerFinished(EventTimer timer)
		{
			mTimerList.Remove(timer);
		}

		/// <summary>
		/// 注册一个定时器
		/// </summary>
		/// <param name="time">定时器时长</param>
		/// <param name="parameters">参数</param>
		/// <param name="callback">回调</param>
		/// <param name="isInvoked">是否invoke</param>
		/// <returns>定时器</returns>
		public EventTimer RegisterTimer(float time, object[] parameters, EventTimer.OnTimerFinishCallback onTimerFinishCallback, int triggerTime = 0, Action<EventTimer> callback = null)
		{
			EventTimer timer = new EventTimer(time, parameters, onTimerFinishCallback, triggerTime, callback);
			mTimerList.Add(timer);

			timer.RegisterTimerFinish(OnTimerFinished);
			return timer;
		}

		public bool UnRegisterTimer(EventTimer timer)
		{
			return mTimerList.Remove(timer);
		}
	}
}

public class EventTimer : IModelEvent
{
	public readonly float Time;
	public readonly object[] Parameters;
	public readonly Action<EventTimer> Callback;

	public float CalTime { get; private set; }

	public EventTimer(float time, object[] parameters, OnTimerFinishCallback onTimerFinishCallback, int triggerTime = 0, Action<EventTimer> callback = null) : base(triggerTime)
	{
		Time = time;
		Parameters = parameters;
		Callback = callback;
		this.onTimerFinishCallback = onTimerFinishCallback;

		ModelEventType = EModelEventType.EModelEventType_Timer;
	}

	/// <summary>
	/// 推进一段时间
	/// </summary>
	/// <param name="deltaTime"></param>
	public void Refresh(float deltaTime)
	{
		CalTime += deltaTime;
		if (CalTime < Time)
			return;

		DoTrigger();
		CalTime = 0;
	}

	protected override void DoCallback() { if (null != Callback) Callback.Invoke(this); }

	protected override void DoFinish()
	{
		base.DoFinish();

		if (null != onTimerFinishCallback)
			onTimerFinishCallback.Invoke(this);
	}

	public delegate void OnTimerFinishCallback(EventTimer timer);
	private OnTimerFinishCallback onTimerFinishCallback;

	public void RegisterTimerFinish(OnTimerFinishCallback callback) { onTimerFinishCallback += callback; }
}
