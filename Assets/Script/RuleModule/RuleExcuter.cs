using System;
using System.Collections.Generic;

using CJC.Framework.IO;


namespace CJC.Framework.Rule.Base
{
	public abstract class RuleExcuter : CRuleBranch
	{
		public bool IsBlocked { get; protected set; }
		public bool IsMultiRoute { get; protected set; }
		protected Dictionary<string, RuleRoute> RuleRoutes;
		protected Dictionary<string, string> Attributes;
		public void DoExecute(IModelData target)
		{
			if (IsBlocked) RuleManager.Instance.RunningIn(target, this);
			OnExecute(target);
		}

		public override void OnInit(CRule rule, XmlData data)
		{
			base.OnInit(rule, data);
			Attributes = data.Attributes;

			RuleRoutes = new Dictionary<string, RuleRoute>();
			if (!IsMultiRoute)
			{
				RuleRoute route = new RuleRoute();
				route.OnInit(ERuleKey.RouteSingle, this, data);
				RuleRoutes.Add(ERuleKey.RouteSingle, route);
			}
			else
			{
				foreach (var exeData in data.Childs)
				{
					string routeName = exeData.Node;
					RuleRoute route = new RuleRoute();
					route.OnInit(routeName, this, exeData);
					RuleRoutes.Add(routeName, route);
				}
			}
		}
		protected abstract void OnExecute(IModelData target);

		protected void DoNext(IModelData target)
		{
			if (!IsMultiRoute)
				return;
			DoRoute(target, ERuleKey.RouteSingle);
		}

		protected void DoRoute(IModelData target, string routeName)
		{
			RuleRoutes[routeName].DoNext(target);
		}

		// 离开这个组件
		public void DoExit(IModelData target)
		{
			ClearCache(target);
			if (IsBlocked) RuleManager.Instance.RunningOver(target, this);
		}

		// 包括 清除缓存 清除监听表等
		public virtual void ClearCache(IModelData target) { }

		public bool Export(XmlData data)
		{
			ExportAttributes(data);
			foreach (var route in RuleRoutes.Values)
				if (!route.Export(data))
					return false;
			return true;
		}

		protected abstract bool ExportAttributes(XmlData data);
		protected bool ExportAttribute(XmlData data, string attributeName)
		{
			string value;
			if (!Attributes.TryGetValue(attributeName, out value))
				return false;

			data.SetAttribute(attributeName, value);
			return true;
		}
	}

	public class RuleRoute
	{
		private string RouteName;
		public RuleExcuter Executer { get; private set; }
		private List<RuleExcuter> NextExcuters;

		public void DoNext(IModelData target)
		{
			foreach (var excuter in NextExcuters)
				excuter.DoExecute(target);
		}

		public void OnInit(string routeName, RuleExcuter parent, XmlData data)
		{
			RouteName = routeName;

			Executer = parent;
			NextExcuters = new List<RuleExcuter>();
			foreach (var exeData in data.Childs)
			{
				Type type = Type.GetType("CJC.Framework.Rule." + exeData.Node);
				RuleExcuter executer = Activator.CreateInstance(type) as RuleExcuter;
				executer.OnInit(Executer.mRule, exeData);
				NextExcuters.Add(executer);
			}
		}

		public bool Export(XmlData data)
		{
			if(Executer.IsMultiRoute)
			{
				data.SetNodeName(RouteName);
				XmlData newData = new XmlData();
				data.AddChild(newData);
				data = newData;
			}
			foreach(var executer in NextExcuters)
			{
				if (!executer.Export(data))
					return false;
			}
			return true;
		}
	}
}