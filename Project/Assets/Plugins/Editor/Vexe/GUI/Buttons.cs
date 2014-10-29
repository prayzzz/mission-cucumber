using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;

namespace Vexe.Editor.Framework.GUIs
{
	public enum MiniButtonStyle { Left, Mid, Right, ModLeft, ModMid, ModRight }

	public partial class GLWrapper
	{
		public const MiniButtonStyle DefaultMiniStyle = MiniButtonStyle.Mid;
		public const MiniButtonStyle DefaultModStyle = MiniButtonStyle.ModMid;
		public static GUILayoutOption[] DefaultMiniOption;
		private static GUIStyle DefaultButtonStyle { get { return GUI.skin.button; } }

		static GLWrapper()
		{
			DefaultMiniOption = new GUILayoutOption[2]
			{
				GUILayout.Width(GUIConstants.kDefaultMiniWidth),
				GUILayout.Height(GUIConstants.kDefaultMiniHeight)
			};
		}

		public bool Button(string text)
		{
			return Button(text, "");
		}
		public bool Button(string text, GUIStyle style)
		{
			return Button(text, "", style);
		}

		public bool Button(string text, GUIStyle style, params GUILayoutOption[] option)
		{
			return Button(GetContent(text), style, option);
		}

		public bool Button(string text, params GUILayoutOption[] option)
		{
			return Button(text, "", option);
		}

		public bool Button(string text, string tooltip)
		{
			return Button(text, tooltip, (GUILayoutOption[])null);
		}

		public bool Button(string text, string tooltip, params GUILayoutOption[] option)
		{
			return Button(text, tooltip, DefaultButtonStyle, option);
		}

		public bool Button(string text, string tooltip, GUIStyle style)
		{
			return Button(text, tooltip, style, null);
		}

		public bool Button(string text, string tooltip, GUIStyle style, params GUILayoutOption[] option)
		{
			return Button(GetContent(text, tooltip), style, option);
		}

		public bool Button(GUIContent content)
		{
			return Button(content, DefaultButtonStyle, null);
		}

		public bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
		{
			return GUILayout.Button(content, style, option);
		}

		public bool InvisibleButton(params GUILayoutOption[] option)
		{
			return Button(GUIContent.none, GUIStyle.none, option);
		}

		public bool InvisibleButton()
		{
			return InvisibleButton(null);
		}

		public bool MiniButton(string text)
		{
			return MiniButton(text, "");
		}
		public bool MiniButton(string text, string tooltip)
		{
			return MiniButton(text, tooltip, DefaultMiniStyle);
		}
		public bool MiniButton(string text, MiniButtonStyle style)
		{
			return MiniButton(text, style, DefaultMiniOption);
		}
		public bool MiniButton(string text, params GUILayoutOption[] option)
		{
			return MiniButton(text, DefaultMiniStyle, option);
		}

		public bool MiniButton(string text, MiniButtonStyle style, params GUILayoutOption[] option)
		{
			return MiniButton(text, "", style, option);
		}
		public bool MiniButton(string text, string tooltip, MiniButtonStyle style)
		{
			return MiniButton(text, tooltip, style, DefaultMiniOption);
		}
		public bool MiniButton(string text, string tooltip, MiniButtonStyle style, params GUILayoutOption[] option)
		{
			return MiniButton(GetContent(text, tooltip), style, option);
		}
		public bool MiniButton(GUIContent content, MiniButtonStyle style)
		{
			return MiniButton(content, style, DefaultMiniOption);
		}
		public bool MiniButton(GUIContent content, MiniButtonStyle style, params GUILayoutOption[] option)
		{
			return MiniButton(content, GUIHelper.GetStyle(style), option);
		}
		public bool MiniButton(string text, string tooltip, GUIStyle style, params GUILayoutOption[] option)
		{
			return MiniButton(GetContent(text, tooltip), style, option);
		}
		public bool MiniButton(string text, string tooltip, GUIStyle style)
		{
			return MiniButton(GetContent(text, tooltip), style, DefaultMiniOption);
		}
		public bool MiniButton(string text, string tooltip, params GUILayoutOption[] option)
		{
			if (option == null)
				return MiniButton(GetContent(text, tooltip), GUIHelper.GetStyle(DefaultMiniStyle), GUILayout.Height(GUIConstants.kDefaultMiniHeight));
			return MiniButton(GetContent(text, tooltip), GUIHelper.GetStyle(DefaultMiniStyle), option);
		}
		public bool MiniButton(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
		{
			return Button(content, style, option);
		}

		public bool AddButton(string target)
		{
			return AddButton(target, DefaultModStyle);
		}
		public bool AddButton(string target, MiniButtonStyle style)
		{
			return MiniButton("+", "Add a new " + target, style);
		}

		public bool ClearButton(string target)
		{
			return ClearButton(target, DefaultModStyle);
		}
		public bool ClearButton(string target, MiniButtonStyle style)
		{
			return MiniButton("x", "Clear " + target, style);
		}

		public bool RemoveButton(string target)
		{
			return RemoveButton(target, DefaultModStyle);
		}
		public bool RemoveButton(string target, MiniButtonStyle style)
		{
			return MiniButton("-", "Remove " + target, style);
		}

		public bool MoveUpButton(MiniButtonStyle style)
		{
			return MiniButton("▲", "Move element up", style);
		}
		public bool MoveUpButton()
		{
			return MoveUpButton(DefaultMiniStyle);
		}
		public bool MoveUpButton<T>(List<T> list, int atIndex, MiniButtonStyle style)
		{
			var move = MoveUpButton(style);
			if (move)
				list.MoveElementUp(atIndex);
			return move;
		}
		public bool MoveUpButton<T>(List<T> list, int atIndex)
		{
			return MoveUpButton(list, atIndex, DefaultMiniStyle);
		}

		public bool MoveDownButton(MiniButtonStyle style)
		{
			return MiniButton("▼", "Move element down", style);
		}
		public bool MoveDownButton()
		{
			return MoveDownButton(DefaultMiniStyle);
		}
		public bool MoveDownButton<T>(List<T> list, int atIndex, MiniButtonStyle style)
		{
			var move = MoveDownButton(style);
			if (move)
				list.MoveElementDown(atIndex);
			return move;
		}

		public bool MoveDownButton<T>(List<T> list, int atIndex)
		{
			return MoveDownButton(list, atIndex, DefaultMiniStyle);
		}

		public bool ToggleButton(bool value, string on, string off, string whatToToggle, MiniButtonStyle style = MiniButtonStyle.Mid)
		{
			var btn = MiniButton(value ? on : off, "Toggle " + whatToToggle, style);
			return btn ? !value : value;
		}

		public bool FoldoutButton(bool value, string whatToFoldout, MiniButtonStyle style = MiniButtonStyle.Mid)
		{
			return ToggleButton(value, GUIHelper.Folds.DefaultFoldSymbol, GUIHelper.Folds.DefaultExpandSymbol, whatToFoldout, style);
		}

		public bool CheckButton(bool value, string whatToToggle)
		{
			return CheckButton(value, whatToToggle, DefaultMiniStyle);
		}

		public bool CheckButton(bool value, string whatToToggle, MiniButtonStyle style)
		{
			return ToggleButton(value, "✔", "", whatToToggle, style);
		}

		public bool RefreshButton(params GUILayoutOption[] option)
		{
			return Button("↶", "Refresh", GUIHelper.RefreshButtonStyle, option);
		}

		public bool RefreshButton()
		{
			return RefreshButton(DefaultMiniOption);
		}

		public bool SelectionButton(string whatToSelect, params GUILayoutOption[] option)
		{
			return Button("◉", "Select " + whatToSelect, GUIHelper.SelectionButtonStyle, option);
		}

		public bool SelectionButton(string whatToSelect)
		{
			return SelectionButton(whatToSelect, DefaultMiniOption);
		}

		public bool SelectionButton()
		{
			return SelectionButton("");
		}

		public bool InspectButton(GameObject go, MiniButtonStyle style, params GUILayoutOption[] option)
		{
			var inspect = MiniButton("⏎", "Inspect", style, option);
			if (inspect) EditorHelper.InspectTarget(go);
			return inspect;
		}

		public bool InspectButton(GameObject go, MiniButtonStyle style)
		{
			return InspectButton(go, style, DefaultMiniOption);
		}

		public bool InspectButton(GameObject go, params GUILayoutOption[] option)
		{
			return InspectButton(go, DefaultMiniStyle, option);
		}

		public bool InspectButton(GameObject go)
		{
			return InspectButton(go, DefaultMiniStyle, DefaultMiniOption);
		}
	}
}
