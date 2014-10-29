using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		private static int indentLevel;
		private int previousLevel;
		private bool guiState;
		private Color guiColor;

		public static int CurrentIndentLevel
		{
			get { return indentLevel; }
			set { indentLevel = value; }
		}

		public static int NextIndentLevel
		{
			get { return CurrentIndentLevel + 1; }
		}

		public void _beginWidth(float width)
		{
			EditorGUIUtility.labelWidth = width;
		}
		public void _endWidth()
		{
			EditorGUIUtility.labelWidth = 0;
		}
		public void _beginV(GUIStyle style)
		{
			EditorGUILayout.BeginVertical(style);
		}
		public void _beginH(GUIStyle style)
		{
			EditorGUILayout.BeginHorizontal(style);
		}
		public void _endV()
		{
			EditorGUILayout.EndVertical();
		}
		public void _endH()
		{
			EditorGUILayout.EndHorizontal();
		}
		public void _beginState(bool newState)
		{
			guiState = GUI.enabled;
			GUI.enabled = newState;
		}
		public void _endState()
		{
			GUI.enabled = guiState;
		}
		public void _beginColor(Color? color)
		{
			_beginColor(color.HasValue ? color.Value : Color.white);
		}
		public void _beginColor(Color color)
		{
			guiColor = GUI.color;
			GUI.color = color;
		}
		public void _endColor()
		{
			GUI.color = guiColor;	
		}
		public void _beginV()
		{
			_beginV(GUIStyle.none);
		}
		public void _beginH()
		{
			_beginH(GUIStyle.none);
		}
		public void _beginIndent()
		{
			_beginIndent(GUIStyle.none);
		}
		public void _beginIndent(GUIStyle style)
		{
			_beginIndent(style, NextIndentLevel);
		}
		public void _beginIndent(GUIStyle style, int indentLevel)
		{
			_beginH();
			previousLevel = CurrentIndentLevel;
			Space(indentLevel * GUIConstants.kIndentAmount);
			_beginV(style);
		}
		public void _endIndent()
		{
			_endV();
			CurrentIndentLevel = previousLevel;
			_endH();
		}
		public void _beginChange()
		{
			EditorGUI.BeginChangeCheck();
		}
		public bool _endChange()
		{
			return EditorGUI.EndChangeCheck();
		}
	}
}