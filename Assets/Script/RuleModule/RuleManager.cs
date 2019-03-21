using System.Collections.Generic;

using CJC.Framework.IO;
using CJC.Framework.Rule.Base;

namespace CJC.Framework.Rule
{
	public class RuleManager
	{
		private RuleManager()
		{
			XmlData data = XmlTool.LoadFile(ERuleKey.RuleConfigUrl);

			RuleRepertory = new Dictionary<int, CRule>();

			foreach (var ruleData in data.Childs)
			{
				CRule rule = new CRule(ruleData);
				RuleRepertory.Add(rule.RuleID, rule);
			}
		}

		private static RuleManager instance;
		public static RuleManager Instance
		{
			get
			{
				if (null == instance)
					instance = new RuleManager();
				return instance;
			}
		}

		public readonly Dictionary<int, CRule> RuleRepertory;

		private CRule GetRule(int ruleID)
		{
			CRule rule;
			RuleRepertory.TryGetValue(ruleID, out rule);
			return rule;
		}

		#region 场景中运行的规则信息，都记录在这里
		private Dictionary<IModelData, Dictionary<int, Dictionary<int, RuleExcuter>>> mRunningRules = new Dictionary<IModelData, Dictionary<int, Dictionary<int, RuleExcuter>>>();	// -- 仅记录阻塞

		public bool StartRule(int ruleID, IModelData target)
		{
			if (SearchRuleIsRunning(ruleID, target))
				return false;

			CRule rule = GetRule(ruleID);
			if (null == rule)
				return false;

			rule.StartRule(target);

			Dictionary<int, Dictionary<int, RuleExcuter>> runningRuleInfo;
			if (!mRunningRules.TryGetValue(target, out runningRuleInfo))
			{
				runningRuleInfo = new Dictionary<int, Dictionary<int, RuleExcuter>>();
				mRunningRules.Add(target, runningRuleInfo);
			}
			Dictionary<int, RuleExcuter> runningExcutersInfo;
			if (!runningRuleInfo.TryGetValue(ruleID, out runningExcutersInfo))
			{
				runningExcutersInfo = new Dictionary<int, RuleExcuter>();
				runningRuleInfo.Add(ruleID, runningExcutersInfo);
			}
			return true;
		}

		public bool StopRule(int ruleID, IModelData target)
		{
			var excuterList = SearchRunningExcuterList(target, ruleID);
			if (null == excuterList)
				return false;

			foreach (var executer in excuterList.Values)
				executer.ClearCache(target);

			mRunningRules[target].Remove(ruleID);
			return true;
		}

		public void ClearRules(IModelData target)
		{
			Dictionary<int, Dictionary<int, RuleExcuter>> runningRuleInfo;
			if (!mRunningRules.TryGetValue(target, out runningRuleInfo))
				return;

			foreach (var runningExecuterInfo in runningRuleInfo.Values)
				foreach (var executer in runningExecuterInfo.Values)
					executer.ClearCache(target);

			mRunningRules.Remove(target);
		}

		public void RunningOver(IModelData target, RuleExcuter excuter)
		{
			Dictionary<int, RuleExcuter> runningExcutersInfo = SearchRunningExcuterList(target, excuter.RuleID);
			if (null == runningExcutersInfo)
			{
				ToolDebug.Error();
				return;
			}

			if (!runningExcutersInfo.Remove(excuter.BranchID))
				ToolDebug.Error();
		}

		public void RunningIn(IModelData target, RuleExcuter excuter)
		{
			Dictionary<int, RuleExcuter> runningExcutersInfo = SearchRunningExcuterList(target, excuter.RuleID);
			if (null == runningExcutersInfo)
			{
				ToolDebug.Error();
				return;
			}

			if (runningExcutersInfo.ContainsKey(excuter.BranchID))
			{
				ToolDebug.Error();
				return;
			}
			runningExcutersInfo.Add(excuter.BranchID, excuter);
		}

		private Dictionary<int, RuleExcuter> SearchRunningExcuterList(IModelData target, int ruleID)
		{
			Dictionary<int, Dictionary<int, RuleExcuter>> runningRuleInfo;
			if (!mRunningRules.TryGetValue(target, out runningRuleInfo))
				return null;

			Dictionary<int, RuleExcuter> runningExcutersInfo;
			if (!runningRuleInfo.TryGetValue(ruleID, out runningExcutersInfo))
				return null;

			return runningExcutersInfo;
		}

		private bool SearchRuleIsRunning(int ruleID, IModelData target)
		{
			Dictionary<int, Dictionary<int, RuleExcuter>> runningRuleInfo;
			if (!mRunningRules.TryGetValue(target, out runningRuleInfo))
				return false;

			return runningRuleInfo.ContainsKey(ruleID);
		}
		#endregion
	}
}

public class ERuleKey
{
	public const string ID = "id";				// 组件id
	public const string Main = "main";          // 规则入口节点名
	public const string RouteSingle = "";
	public const string RuleConfigUrl = "Config/ruleconfig.xml";     // 规则配置路径

	// ExeListenTimer
	public const string TriggerCount = "";      // 监听器触发次数
	public const string TimerTime = "";         // 计时器计时时间
	public const string RouteTrigger = "";
	public const string RouteFinish = "";

	// ExeIfComparison
	public const string Param1 = "";
	public const string Param2 = "";
	public const string CompareSymbol = "";
	public const string RouteIf = "";
	public const string RouteElse = "";


}