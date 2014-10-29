using UnityEditor;
using UnityEngine;
using Vexe.Editor.Framework;
using System;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public void Box(string text, params GUILayoutOption[] option)
		{
			Box(text, UnityEngine.GUI.skin.box, option);
		}
		public void Box(string text, GUIStyle style, params GUILayoutOption[] option)
		{
			Box(GetContent(text), style, option);
		}

		public void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] option)
		{
			GUILayout.Box(content, style, option);
		}

		public void HelpBox(string message)
		{
			HelpBox(message, MessageType.Info);
		}

		public void HelpBox(string message, MessageType type)
		{
			EditorGUILayout.HelpBox(message, type);
		}
	}
}
