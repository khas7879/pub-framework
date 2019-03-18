﻿using System;
using CJC.Framework.IO;
using System.Collections.Generic;
using CJC.Framework.Rule;

namespace CJC.Framework.Rule.Base
{
	public class CRule
	{
		public readonly int RuleID;
		public readonly Dictionary<int, RuleExcuter> EnteranceExcuters; 

		public CRule(XmlData data)
		{
			RuleID = ToolParser.IntParse(data.GetAttribute(ERuleKey.ID));
			XmlData main = data.Childs[ERuleKey.Main];

			EnteranceExcuters = new Dictionary<int, RuleExcuter>();
			foreach (var exeData in main.Childs.Values)
			{
				Type type = Type.GetType("CJC.Framework.Rule." + exeData.Node);
				RuleExcuter branch = Activator.CreateInstance(type) as RuleExcuter;
				branch.OnInit(this, exeData);

				EnteranceExcuters.Add(branch.BranchID, branch);
			}
		}

		public void StartRule(IModelData target)
		{
			foreach(var executer in EnteranceExcuters.Values)
			{
				executer.DoExecute(target);
			}
		}
	}

	public abstract class CRuleBranch
	{
		public CRule mRule;
		public int RuleID { get { return mRule.RuleID; } }
		public int BranchID { get; protected set; }
		public virtual void OnInit(CRule rule, XmlData data)
		{
			mRule = rule;
			BranchID = ToolParser.IntParse(data.GetAttribute(ERuleKey.ID));
		}
	}
}