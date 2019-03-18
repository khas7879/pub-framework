using System.Collections.Generic;

using CJC.Framework.Rule.Base;

namespace CJC.Framework.Rule
{
	public class ExeListenTimer : RuleExcuter
	{
		private static Dictionary<ExeListenTimer, EventTimer> TimerDic = new Dictionary<ExeListenTimer, EventTimer>();
		public ExeListenTimer() : base() { IsBlocked = true; IsMultiRoute = true; }

		protected override void OnExecute(IModelData target)
		{
			int triggerCount = ToolParser.IntParse(Attributes[ERuleKey.TriggerCount]);
			float timerTime = ToolParser.FloatParse(Attributes[ERuleKey.TimerTime]);
			EventTimer timer = target.RegisterTimer(timerTime, new object[] { target }, OnTimerFinish, triggerCount, OnTimerTrigger);
			TimerDic.Add(this, timer);
		}

		private void OnTimerTrigger(EventTimer timer)
		{
			IModelData target = timer.Parameters[0] as IModelData;

			DoRoute(target, ERuleKey.RouteTrigger);
		}

		// 计时器完成 不包含中途退出的情况
		private void OnTimerFinish(EventTimer timer)
		{
			IModelData target = timer.Parameters[0] as IModelData;

			DoExit(target);
			DoRoute(target, ERuleKey.RouteFinish);
		}

		public override void ClearCache(IModelData target)
		{
			base.ClearCache(target);

			EventTimer timer;
			if (TimerDic.TryGetValue(this, out timer))
			{
				target.UnRigisterTimer(timer);
				TimerDic.Remove(this);
			}
		}
	}
}
