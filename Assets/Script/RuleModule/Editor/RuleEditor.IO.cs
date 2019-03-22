/*
* 导出规则一共导出两份
* 一份为编辑器文件(保存)，一份为项目所用规则文件(导出)。
*/


using UnityEngine;
using System.Collections;
using UnityEditor;
using CJC.Framework.IO;
using CJC.Framework.Rule.Base;
using System.Collections.Generic;

public partial class RuleEditor
{
	private void InitData()
	{
		//if (null != mRuleList)
		//	return;

		mRuleList = new List<RuleObject>();
		XmlData root = XmlTool.GetXmlData(ERuleParam4Editor.RuleConfigUrlForEditor);
		if (null == root)
			return;

		foreach (var data in root.Childs)
		{
			RuleObject ruleObject = new RuleObject();
			if (!ruleObject.XmlDeserilize(data))
				continue;

			mRuleList.Add(ruleObject);
		}
	}
	private void SaveSingleRule(Object obj)
	{
		if (null == SelectedRuleObject)
			return;

		string tip = _SaveSingleRule(SelectedRuleObject) ? @"编辑文件保存成功！" : @"编辑文件保存失败！";
		EditorUtility.DisplayDialog(@"提示！", tip, "Ok");
	}

	private void SaveAllRuleObject(Object obj)
	{
		string tip = _SaveAllRuleObject() ? @"编辑文件保存成功！" : @"编辑文件保存失败！";
		EditorUtility.DisplayDialog(@"提示！", tip, "Ok");
	}

	private void OutputSingleRule(Object obj)
	{
		if (null == SelectedRuleObject)
			return;

		string tip = _OutputSingleRule(SelectedRuleObject.RuleHost) ? @"配置文件导出成功！" : @"配置文件导出失败！";
		EditorUtility.DisplayDialog(@"提示！", tip, "Ok");
	}

	private void OutputAllRule(Object obj)
	{
		string tip = _OutputAllRule() ? @"配置文件导出成功！" : @"配置文件导出失败！";
		EditorUtility.DisplayDialog(@"提示！", tip, "Ok");
	}

	private bool _SaveSingleRule(RuleObject ruleObject)
	{
		return XmlTool.ReplaceXmlData(ERuleParam4Editor.RuleConfigUrlForEditor, ruleObject, string.Format("root/node[@id='{0}']", ruleObject.RuleHost.RuleID));
	}

	private bool _SaveAllRuleObject()
	{
		XmlData root = new XmlData();
		foreach (var ruleObject in mRuleList)
		{
			XmlData ruleData = new XmlData();
			if (!ruleObject.XmlSerilize(ref ruleData))
				return false;

			root.AddChild(ruleData);
		}
		XmlTool.ExportFile(ERuleParam4Editor.RuleConfigUrlForEditor, root);
		return true;
	}

	private bool _OutputSingleRule(CRule rule)
	{
		return XmlTool.ReplaceXmlData(ERuleParam4Editor.RuleConfigUrl, rule, string.Format("root/node[@id='{0}']", rule.RuleID));
	}

	private bool _OutputAllRule()
	{
		XmlData root = new XmlData();
		foreach (var ruleObject in mRuleList)
		{
			XmlData ruleData = new XmlData();
			if (!ruleObject.RuleHost.XmlSerilize(ref ruleData))
				return false;

			root.AddChild(ruleData);
		}
		XmlTool.ExportFile(ERuleParam4Editor.RuleConfigUrl, root);
		return true;
	}

	private class ERuleParam4Editor
	{
		public const string RuleConfigUrl = "config/ruleconfig.xml";
		public const string RuleConfigUrlForEditor = "config/Editor/ruleconfig.xml";
	}
}