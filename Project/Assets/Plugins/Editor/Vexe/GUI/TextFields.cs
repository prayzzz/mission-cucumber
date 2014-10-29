using Fasterflect;
using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		private MethodInvoker toolbarSearchField;

		public GLWrapper()
		{
			toolbarSearchField = typeof(EditorGUILayout).DelegateForCallMethod("ToolbarSearchField", Flags.StaticPrivate, typeof(string), typeof(GUILayoutOption[]));
		}

		public string TextField(string value)
		{
			return TextField(string.Empty, value);
		}
		public string TextField(string label, string value)
		{
			return TextField(label, value, (GUILayoutOption[])null);
		}
		public string TextField(string label, string value, params GUILayoutOption[] option)
		{
			return TextField(label, value, string.Empty, option);
		}
		public string TextField(string label, string value, string tooltip)
		{
			return TextField(label, value, tooltip, null);
		}
		public string TextField(string label, string value, string tooltip, params GUILayoutOption[] option)
		{
			return TextField(GetContent(label, tooltip), value, option);
		}
		public string TextField(GUIContent content, string value, params GUILayoutOption[] option)
		{
			return TextField(content, value, EditorStyles.numberField, option);
		}
		public string TextField(GUIContent content, string value, GUIStyle style, params GUILayoutOption[] option)
		{
			return EditorGUILayout.TextField(content, value, style, option);
		}

		public string ToolbarSearchField(string current)
		{
			return ToolbarSearchField(current, null);
		}
		public string ToolbarSearchField(string current, params GUILayoutOption[] options)
		{
			return toolbarSearchField(null, current, options) as string;
		}
	}
}
