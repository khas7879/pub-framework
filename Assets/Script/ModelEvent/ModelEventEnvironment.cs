using System;

using CJC.Framework.Event.Base;

public class ModelEventEnvironment : IModelEvent
{
	public ModelEventEnvironment(EModelEventEnvironment type) { this.ModelEventType = EModelEventType.EModelEventType_Environment; EventType = type; }
	public ModelEventEnvironment(EModelEventEnvironment type, int triggerTime):base(triggerTime) { this.ModelEventType = EModelEventType.EModelEventType_Environment; EventType = type; }

	public readonly EModelEventEnvironment EventType;

	protected Action<ModelEventEnvironment> mCallBack;
	public void RegisterCallback(Action<ModelEventEnvironment> callback) { mCallBack += callback; }
	public void UnregisterCallback(Action<ModelEventEnvironment> callback) { mCallBack -= callback; }

	protected override void DoCallback()
	{
		if (null != mCallBack)
			mCallBack.Invoke(this);
	}
}

public enum EModelEventEnvironment
{

}
