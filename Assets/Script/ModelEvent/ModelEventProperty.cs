using System;
using CJC.Framework.Event.Base;

namespace CJC.Framework.Event
{
	public abstract class ModelEventProperty : IModelEvent
	{
		public ModelEventProperty() : base() { ModelEventType = EModelEventType.EModelEventType_Property; }
		public ModelEventProperty(int triggerTime) : base(triggerTime) { ModelEventType = EModelEventType.EModelEventType_Property; }
	}

	public class CInt : ModelEventProperty
	{
		public CInt() : base() { }
		public CInt(int value) : base() { newVal = value; }
		public CInt(int value, Action<CInt> callback):base() { newVal = value;  AddPropChangedCB(callback); }
		public CInt(int value, Action<CInt> callback, int triggerTime) : base(triggerTime) { newVal = value; AddPropChangedCB(callback); }

		public int oldVal { get; private set; }
		public int newVal { get; private set; }

		protected Action<CInt> mPropertyChangedCallback;
		public void AddPropChangedCB(Action<CInt> callback){ mPropertyChangedCallback += callback; }

		public void RemovePropChangedCB(Action<CInt> callback) { mPropertyChangedCallback -= callback; }

		protected override void DoCallback()
		{
			if (null == mPropertyChangedCallback)
				return;
			mPropertyChangedCallback.Invoke(this);
		}

		public int Value
		{
			get { return newVal; }
			set
			{
				if (newVal == value) return;
				DoTrigger();
				oldVal = newVal;
				newVal = value;
			}
		}
	}

	public class CFloat : ModelEventProperty
	{
		public CFloat() : base() { }
		public CFloat(float value, Action<CFloat> callback):base() { newVal = value; AddPropChangedCB(callback); }
		public CFloat(float value, Action<CFloat> callback, int triggerTime) : base(triggerTime) { newVal = value; AddPropChangedCB(callback); }

		public float oldVal { get; private set; }
		public float newVal { get; private set; }

		protected Action<CFloat> mPropertyChangedCallback;
		public void AddPropChangedCB(Action<CFloat> callback) { mPropertyChangedCallback += callback; }

		public void RemovePropChangedCB(Action<CFloat> callback) { mPropertyChangedCallback -= callback; }

		protected override void DoCallback()
		{
			if (null == mPropertyChangedCallback)
				return;
			mPropertyChangedCallback.Invoke(this);
		}

		public float Value
		{
			get { return newVal; }
			set
			{
				if (newVal == value) return;
				DoTrigger();
				oldVal = newVal;
				newVal = value;
			}
		}
	}

	public class CBool : ModelEventProperty
	{
		public CBool() : base() { }
		public CBool(bool value, Action<CBool> callback) : base() { mVal = value; AddPropChangedCB(callback); }
		public CBool(bool value, Action<CBool> callback, int triggerTime) : base(triggerTime) { mVal = value; AddPropChangedCB(callback); }

		private bool mVal;
		protected Action<CBool> mPropertyChangedCallback;
		public void AddPropChangedCB(Action<CBool> callback) { mPropertyChangedCallback += callback; }

		public void RemovePropChangedCB(Action<CBool> callback) { mPropertyChangedCallback -= callback; }

		protected override void DoCallback()
		{
			if (null == mPropertyChangedCallback)
				return;
			mPropertyChangedCallback.Invoke(this);
		}

		public bool Value
		{
			get { return mVal; }
			set
			{
				if (mVal == value) return;
				DoTrigger();
				mVal = value;
			}
		}
	}

	public class CString : ModelEventProperty
	{
		public CString() : base() { }
		public CString(string value, Action<CString> callback) : base() { newVal = value; AddPropChangedCB(callback); }
		public CString(string value, Action<CString> callback, int triggerTime) : base(triggerTime) { newVal = value; AddPropChangedCB(callback); }

		public string oldVal { get; private set; }
		public string newVal { get; private set; }

		protected Action<CString> mPropertyChangedCallback;
		public void AddPropChangedCB(Action<CString> callback) { mPropertyChangedCallback += callback; }

		public void RemovePropChangedCB(Action<CString> callback) { mPropertyChangedCallback -= callback; }

		protected override void DoCallback()
		{
			if (null == mPropertyChangedCallback)
				return;
			mPropertyChangedCallback.Invoke(this);
		}

		public string Value
		{
			get { return newVal; }
			set
			{
				if (newVal == value) return;
				DoTrigger();
				oldVal = newVal;
				newVal = value;
			}
		}
	}
}