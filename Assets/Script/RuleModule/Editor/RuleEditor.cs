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

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		ShowRuleList();
		ShowSelected();
		EditorGUILayout.EndHorizontal();

		if(CheckInEditorWindow())
			UpdateEvent();
	}


	private float GetCutoffLineX() { return position.width * 0.2f; }

	private bool CheckInEditorWindow()
	{
		Vector2 mousePosition = Event.current.mousePosition;
		return mouseOverWindow == this && mousePosition.x >= GetCutoffLineX();
	}

	private void ShowSelected()
	{
		if (null == SelectedRuleObject)
			return;

		PrintRule(SelectedRuleObject);
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

		if (null != mRuleList)
		{
			mScrollViewPosition = EditorGUILayout.BeginScrollView(mScrollViewPosition);
			foreach (var ruleObject in mRuleList)
			{
				bool isSelected = ruleObject.Equals(SelectedRuleObject);
				GUIStyle style = isSelected ? mRuleListSelectedItemStyle : mRuleListItemStyle;

				if (GUILayout.Button(ruleObject.RuleHost.RuleID.ToString(), style))
				{
					SelectRule(ruleObject);
				}
			}
			EditorGUILayout.EndScrollView();
		}

		if(GUILayout.Button("Add"))
		{
			CreateNew(0);
		}
		if (GUILayout.Button("Sort"))
		{
			SortRuleList();
		}
		if(GUILayout.Button("SaveAll"))
		{
			SaveAllRuleObject(null);
		}
		if(GUILayout.Button("ExportAll"))
		{
			OutputAllRule(null);
		}
		EditorGUILayout.EndVertical();

		DeleteRuleObject();
	}
	#endregion

}
