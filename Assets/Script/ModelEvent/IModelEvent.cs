namespace CJC.Framework.Event.Base
{
	public abstract class IModelEvent
	{
		private static long sModelEventUID;
		public readonly long UID;
		public readonly int TriggerTime;
		public EModelEventType ModelEventType { get; protected set; }
		public int RestTime { get; private set; }
		public bool IsInvoked { get { return 0 == TriggerTime; } }
		public bool IsFinish { get { return (!IsInvoked) && RestTime <= 0; } }

		public IModelEvent() { UID = ++sModelEventUID; }
		public IModelEvent(int triggerTime) : base() { TriggerTime = triggerTime; }

		protected void DoTrigger()
		{
			if (IsFinish)
				return;

			RestTime--;
			DoCallback();

			if (IsFinish)
				DoFinish();
		}

		protected abstract void DoCallback();

		protected virtual void DoFinish() { }
	}

	public enum EModelEventType
	{
		EModelEventType_Property,
		EModelEventType_Timer,
		EModelEventType_Environment,
		EModelEventType_Interactive,
	}
}