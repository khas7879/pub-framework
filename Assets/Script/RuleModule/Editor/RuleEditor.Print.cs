using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CJC.Framework.Rule.Base;

public partial class RuleEditor
{
	private const string RuleEditorConfigUrl = "";
	private Dictionary<CRule, RuleObject> mRulePrinterDic;

	private RuleObject SelectedRuleObject
	{
		get {
			if (null == mSelectedRule)
				return null;

			return GetRuleObject(mSelectedRule);
		}
	}

	private RuleObject GetRuleObject(CRule rule)
	{
		RuleObject ruleObject;
		if (!mRulePrinterDic.TryGetValue(rule, out ruleObject))
			return null;

		return ruleObject;
	}

	public void PrintRule(CRule rule)
	{
		if(null == SelectedRuleObject)
			return;

		SelectedRuleObject.Print();
	}

	public void AddRuleObject(CRule newRule)
	{
		if (mRulePrinterDic.ContainsKey(newRule))
			return;

	}

	public void DeleteRuleObject(CRule deletedRule)
	{
		mRulePrinterDic.Remove(deletedRule);
	}

	// 加载数据文件，主要是位置信息
	public void InitPrinter()
	{
		mRulePrinterDic = new Dictionary<CRule, RuleObject>();
	}

	private RuleObject CreateDefaultRuleObject()
	{
		return null;
	}
}
