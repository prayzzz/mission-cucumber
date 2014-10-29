using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public static GUIContent sharedContent = new GUIContent();
		public static GUIContent GetContent(string text, string tooltip = "")
		{
			sharedContent.text    = text;
			sharedContent.tooltip = tooltip;
			return sharedContent;
		}

		public float FloatField(float value)
		{
			return FloatField("", value);
		}
		public float FloatField(string label, float value)
		{
			return FloatField(label, value, (GUILayoutOption[])null);
		}
		public float FloatField(string label, float value, GUILayoutOption[] option)
		{
			return FloatField(label, value, "", option);
		}
		public float FloatField(string label, float value, string tooltip)
		{
			return FloatField(label, value, tooltip, null);
		}
		public float FloatField(string label, float value, string tooltip, params GUILayoutOption[] option)
		{
			return FloatField(GetContent(label, tooltip), value, option);
		}
		public float FloatField(GUIContent content, float value, params GUILayoutOption[] option)
		{
			return FloatField(content, value, EditorStyles.numberField, option);
		}
		public float FloatField(GUIContent content, float value, GUIStyle style, params GUILayoutOption[] option)
		{
			return EditorGUILayout.FloatField(content, value, option);
		}
	}
}
