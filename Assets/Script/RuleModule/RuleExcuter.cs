using System;
using System.Collections.Generic;

using CJC.Framework.IO;


namespace CJC.Framework.Rule.Base
{
	public abstract class RuleExcuter : CRuleBranch
	{
		protected bool IsBlocked;
		protected bool IsMultiRoute;
		protected Dictionary<string, RuleRoute> RuleRoutes;
		protected Dictionary<string, string> Attributes;
		public void DoExecute(IModelData target)
		{
			if (IsBlocked) RuleManager.Instance.RunningIn(target, this);
			OnExecute(target);
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

		public override void OnInit(CRule rule, XmlData data)
		{
			base.OnInit(rule, data);
			Attributes = data.Attributes;

			RuleRoutes = new Dictionary<string, RuleRoute>();
			if (!IsMultiRoute)
			{
				RuleRoute route = new RuleRoute();
				route.OnInit(this, data);
				RuleRoutes.Add(ERuleKey.RouteSingle, route);
			}
			else 
			{
				foreach (var exeData in data.Childs.Values)
				{
					string routeName = exeData.Node;
					RuleRoute route = new RuleRoute();
					route.OnInit(this, exeData);
					RuleRoutes.Add(routeName, route);
				}
			}
		}
	}

	public class RuleRoute
	{
		public RuleExcuter Executer { get; private set; }
		private List<RuleExcuter> NextExcuters;

		public void DoNext(IModelData target)
		{
			foreach (var excuter in NextExcuters)
				excuter.DoExecute(target);
		}

		public void OnInit(RuleExcuter parent, XmlData data)
		{
			Executer = parent;
			NextExcuters = new List<RuleExcuter>();
			foreach (var exeData in data.Childs.Values)
			{
				Type type = Type.GetType("CJC.Framework.Rule." + exeData.Node);
				RuleExcuter executer = Activator.CreateInstance(type) as RuleExcuter;
				executer.OnInit(Executer.mRule, exeData);
				NextExcuters.Add(executer);
			}
		}
	}
}