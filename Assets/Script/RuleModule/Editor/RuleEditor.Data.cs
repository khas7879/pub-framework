using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CJC.Framework.Rule.Base;
using UnityEditor;
using CJC.Framework.IO;

public partial class RuleEditor
{
	private static List<RuleObject> mRuleList;
	private static RuleObject SelectedRuleObject;

	private void SelectRule(RuleObject ruleObject)
	{
		SelectedRuleObject = ruleObject;
		Selection.activeObject = SelectedRuleObject;
	}

	public void PrintRule(RuleObject ruleObject)
	{
		if(null == SelectedRuleObject)
			return;

		ruleObject.Print();
		ruleObject.Update();
	}

	private RuleObject FindRuleObject(int id)
	{
		return mRuleList.Find((ruleObject) => { return ruleObject.RuleHost.RuleID.Equals(id); });
	}

	private void SortRuleList()
	{
		mRuleList.Sort((rule1, rule2)=>
		{
			int result = rule1.RuleHost.RuleID <= rule2.RuleHost.RuleID ? 0 : 1;
			return result;
		});
	}

	public RuleObject CreateNew(int id)
	{
		RuleObject ruleObject = FindRuleObject(id);
		if(null != ruleObject)
			return ruleObject;

		ruleObject = CreateInstance<RuleObject>();
		ruleObject.RuleHost.SetID(id);
		mRuleList.Add(ruleObject);
		return ruleObject;
	}

	private RuleObject mDeletedRuleObject;
	public void DeleteRuleObject()
	{
		if (null == mDeletedRuleObject)
			return;

		SelectRule(null);
		mRuleList.Remove(mDeletedRuleObject);
		mDeletedRuleObject = null;
	}
}
