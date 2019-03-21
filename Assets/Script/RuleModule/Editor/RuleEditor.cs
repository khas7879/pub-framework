using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

using CJC.Framework.IO;
using CJC.Framework.Rule;
using CJC.Framework.Rule.Base;

public partial class RuleEditor : EditorWindow
{
	[MenuItem("Editor/Rule")]
	public static void OpenWindow()
	{
		RuleEditor window = GetWindow<RuleEditor>();
		window.title = @"规则编辑器";

		window.InitData();
	}

	private static List<CRule> mRules;

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		ShowRuleList();
		ShowSelected();
		EditorGUILayout.EndHorizontal();

		if(CheckInEditorWindow())
			UpdateEvent();
	}

	private void InitData()
	{
		mRules = new List<CRule>(RuleManager.Instance.RuleRepertory.Values);
		InitPrinter();
	}

	private void DeleteRule(CRule rule)
	{
		DeleteRuleObject(rule);
		mRules.Remove(rule);
	}

	private float GetCutoffLineX() { return position.width * 0.2f; }

	private bool CheckInEditorWindow()
	{
		Vector2 mousePosition = Event.current.mousePosition;
		return mouseOverWindow != this && mousePosition.x >= GetCutoffLineX();
	}

	private CRule mSelectedRule;
	private void ShowSelected()
	{
		if (null == mSelectedRule)
			return;

		PrintRule(mSelectedRule);
	}

	#region 显示规则列表
	private Vector2 mScrollViewPosition;

	private GUIStyle _mRuleListItemStyle;
	private GUIStyle _mRuleListSelectedItemStyle;
	private GUIStyle mRuleListItemStyle {
		get {
			if(null == _mRuleListItemStyle)
			{
				_mRuleListItemStyle = new GUIStyle(EditorStyles.label);
				_mRuleListItemStyle.normal.background = Texture2D.blackTexture;
				_mRuleListItemStyle.normal.textColor = Color.white;
			}
			return _mRuleListItemStyle;
		}
	}

	private GUIStyle mRuleListSelectedItemStyle
	{
		get
		{
			if (null == _mRuleListSelectedItemStyle)
			{
				_mRuleListSelectedItemStyle = new GUIStyle(EditorStyles.label);
				_mRuleListSelectedItemStyle.normal.background = Texture2D.whiteTexture;
				_mRuleListSelectedItemStyle.normal.textColor = Color.black;
			}
			return _mRuleListSelectedItemStyle;
		}
	}

	private void ShowRuleList()
	{
		EditorGUILayout.BeginVertical(GUILayout.Width(GetCutoffLineX()));

		if (null != mRules)
		{
			mScrollViewPosition = EditorGUILayout.BeginScrollView(mScrollViewPosition);
			foreach (var rule in mRules)
			{
				bool isSelected = rule.Equals(mSelectedRule);
				GUIStyle style = isSelected ? mRuleListSelectedItemStyle : mRuleListItemStyle;

				if (GUILayout.Button(rule.RuleID.ToString(), style))
				{
					mSelectedRule = rule;
				}
			}
			EditorGUILayout.EndScrollView();
		}

		if(GUILayout.Button("Add"))
		{
			CreateNewRule();
		}
		if (GUILayout.Button("Sort"))
		{
			SortRuleList();
		}
		if(GUILayout.Button("Export"))
		{
			Export();
		}
		EditorGUILayout.EndVertical();
	}

	private void CreateNewRule()
	{
		CRule rule = new CRule(0);
		AddRuleObject(rule);
		mRules.Add(rule);
	}


	private void SortRuleList()
	{
		mRules.Sort(delegate (CRule rule1, CRule rule2)
			{
				int result = rule1.RuleID <= rule2.RuleID ? 0 : 1;
				return result;
			});
	}
	#endregion

	#region 导出规则文件
	private const string RuleFile = "config/ruleconfig.xml";
	private void Export()
	{
		XmlData data;
		if (!TransformRuleList(out data))
		{
			EditorUtility.DisplayDialog(@"提示！", @"规则文件导出失败！", "Ok");
			return;
		}
		XmlTool.ExportFile(RuleFile, data);
		EditorUtility.DisplayDialog(@"提示！", @"规则文件导出成功！", "Ok");
	}

	private bool TransformRuleList(out XmlData data)
	{
		data = new XmlData();

		foreach (var rule in mRules)
		{
			XmlData ruleData = new XmlData();
			ruleData.AddAttribute(ERuleKey.ID, rule.RuleID);

			foreach (var executer in rule.EnteranceExcuters.Values)
			{
				XmlData executerData;
				if (!TransformExecuter2XmlData(executer, out executerData))
					return false;

				ruleData.AddChild(executerData);
			}
			data.AddChild(ruleData);
		}
		return true;
	}

	private bool TransformExecuter2XmlData(RuleExcuter executer, out XmlData data)
	{
		data = new XmlData();
		data.SetNodeName(executer.GetType());

		if (!executer.Export(data))
			return false;

		return true;
	}
	#endregion
}
