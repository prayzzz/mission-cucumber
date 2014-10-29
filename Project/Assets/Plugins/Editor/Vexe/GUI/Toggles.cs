using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public bool Toggle(bool value)
		{
			return Toggle("", value);
		}
		public bool Toggle(string label, bool value)
		{
			return Toggle(label, value, (GUILayoutOption[])null);
		}
		public bool Toggle(string label, bool value, params GUILayoutOption[] option)
		{
			return Toggle(label, value, "", option);
		}
		public bool Toggle(string label, bool value, string tooltip)
		{
			return Toggle(label, value, tooltip, null);
		}
		public bool Toggle(string label, bool value, string tooltip, params GUILayoutOption[] option)
		{
			return Toggle(new GUIContent(label, tooltip), value, option);
		}
		public bool Toggle(GUIContent content, bool value, params GUILayoutOption[] option)
		{
			return Toggle(content, value, EditorStyles.toggle, option);
		}
		public bool Toggle(GUIContent content, bool value, GUIStyle style, params GUILayoutOption[] option)
		{
			return EditorGUILayout.Toggle(content, value, style, option);
		}
	}
}