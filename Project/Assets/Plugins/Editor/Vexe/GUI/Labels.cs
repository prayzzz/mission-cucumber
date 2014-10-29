using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework;
using System;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		/// <summary>
		/// Creates a bold numeric label with a width of NUMERIC_LABEL_WIDTH
		/// </summary>
		public void NumericLabel(int n, GUIStyle style)
		{
			Label(n + "- ", style, GUILayout.Width(GUIConstants.kNumericLabelWidth));
		}

		public void NumericLabel(int n)
		{
			NumericLabel(n, EditorStyles.miniLabel);
		}

		/// <summary>
		/// Creates a NumericLabel, with a TextFieldLabel beside it
		/// </summary>
		public void NumericTextFieldLabel(int n, string label)
		{
			_beginH();
			{
				NumericLabel(n);
				TextFieldLabel(label);
			}
			_endH();
		}

		/// <summary>
		/// Creates a Label with a EditorStyles.textField style
		/// </summary>
		public void TextFieldLabel(string label)
		{
			Label(label, UnityEngine.GUI.skin.textField);
		}

		public void BoldLabel(string text)
		{
			BoldLabel(text, null);
		}

		public void BoldLabel(string text, params GUILayoutOption[] option)
		{
			Label(text, EditorStyles.boldLabel, option);
		}

		public void Label(string text)
		{
			Label(text, (GUILayoutOption[])null);
		}
		public void Label(string text, params GUILayoutOption[] option)
		{
			Label(text, EditorStyles.label, option);
		}
		public void Label(string text, GUIStyle style)
		{
			Label(text, style, null);
		}
		public void Label(string text, GUIStyle style, params GUILayoutOption[] option)
		{
			Label(GetContent(text), style, option);
		}

		public void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
		{
			GUILayout.Label(content, style, option);
		}

		public void PrefixLabel(GUIContent label)
		{
			PrefixLabel(label, GUIStyle.none);
		}
		public void PrefixLabel(string label)
		{
			PrefixLabel(label, GUIStyle.none);
		}
		public void PrefixLabel(GUIContent label, GUIStyle labelStyle)
		{
			PrefixLabel(label, GUIStyle.none, labelStyle);
		}
		public void PrefixLabel(string label, GUIStyle labelStyle)
		{
			PrefixLabel(label, GUIStyle.none, labelStyle);
		}
		public void PrefixLabel(string label, GUIStyle followingStyle, GUIStyle labelStyle)
		{
			PrefixLabel(GetContent(label), followingStyle, labelStyle);
		}
		public void PrefixLabel(GUIContent label, GUIStyle followingStyle, GUIStyle labelStyle)
		{
			EditorGUILayout.PrefixLabel(label, followingStyle, labelStyle);
		}
	}
}