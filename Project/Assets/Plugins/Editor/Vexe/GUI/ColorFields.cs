using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public Color ColorField(Color value)
		{
			return ColorField("", value);
		}
		public Color ColorField(string label, Color value)
		{
			return ColorField(label, value, null);
		}
		public Color ColorField(string label, string tooltip, Color value)
		{
			return ColorField(label, tooltip, value, null);
		}
		public Color ColorField(string label, Color value, params GUILayoutOption[] option)
		{
			return ColorField(label, "", value, option);
		}
		public Color ColorField(string label, string tooltip, Color value, params GUILayoutOption[] option)
		{
			return ColorField(GetContent(label, tooltip), value, option);
		}
		public Color ColorField(GUIContent content, Color value, params GUILayoutOption[] option)
		{
			return EditorGUILayout.ColorField(content, value, option);
		}
	}
}
