using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CJC.Framework.Rule;

public partial class IModelData
{
	public void StartRule(int ruleID) { RuleManager.Instance.StartRule(ruleID, this); }
	public void StopRule(int ruleID) { RuleManager.Instance.StopRule(ruleID, this); }
	public void ClearRule() { RuleManager.Instance.ClearRules(this); }
}
