using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public Rect RectField(Rect value)
		{
			return RectField("", value);
		}
		public Rect RectField(string label, Rect value)
		{
			return RectField(label, value, GUIHelper.MultifieldOption);
		}
		public Rect RectField(string label, string tooltip, Rect value)
		{
			return RectField(label, tooltip, value, GUIHelper.MultifieldOption);
		}
		public Rect RectField(string label, Rect value, params GUILayoutOption[] option)
		{
			return RectField(label, "", value, option);
		}
		public Rect RectField(string label, string tooltip, Rect value, params GUILayoutOption[] option)
		{
			return RectField(new GUIContent(label, tooltip), value, option);
		}
		public Rect RectField(GUIContent content, Rect value, params GUILayoutOption[] option)
		{
			return EditorGUILayout.RectField(content, value, option);
		}
	}
}
