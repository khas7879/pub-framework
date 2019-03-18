using System;
using System.Collections.Generic;

using CJC.Framework.Timer;

public partial class IModelData
{
	private HashSet<EventTimer> mTimerSet;

	public EventTimer RegisterTimer(float time, object[] parameters, EventTimer.OnTimerFinishCallback onTimerFinishCallback, int triggerTime = 0, Action<EventTimer> callback = null)
	{
		callback += OnTimer;
		EventTimer timer = CTimerManager.Instance.RegisterTimer(time, parameters, onTimerFinishCallback, triggerTime, callback);

		if (null == mTimerSet)
			mTimerSet = new HashSet<EventTimer>();

		mTimerSet.Add(timer);
		return timer;
	}

	public void UnRigisterTimer(EventTimer timer)
	{
		if (!mTimerSet.Remove(timer))
			return;

		CTimerManager.Instance.UnRegisterTimer(timer);
	}

	private void OnTimer(EventTimer timer)
	{
		mTimerSet.Remove(timer);
	}

	private void ClearTimer()
	{
		foreach (var timer in mTimerSet)
		{
			CTimerManager.Instance.UnRegisterTimer(timer);
		}
		mTimerSet.Clear();
	}
}
