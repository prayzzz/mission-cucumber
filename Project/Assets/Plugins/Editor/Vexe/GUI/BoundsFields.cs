using UnityEditor;
using UnityEngine;
using Vexe.Editor.Helpers;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public Bounds BoundsField(Bounds value)
		{
			return BoundsField("", value);
		}
		public Bounds BoundsField(string label, Bounds value)
		{
			return BoundsField(label, value, GUIHelper.MultifieldOption);
		}
		public Bounds BoundsField(string label, string tooltip, Bounds value)
		{
			return BoundsField(label, tooltip, value, GUIHelper.MultifieldOption);
		}
		public Bounds BoundsField(string label, Bounds value, params GUILayoutOption[] option)
		{
			return BoundsField(label, "", value, option);
		}
		public Bounds BoundsField(string label, string tooltip, Bounds value, params GUILayoutOption[] option)
		{
			return BoundsField(GetContent(label, tooltip), value, option);
		}
		public Bounds BoundsField(GUIContent content, Bounds value, params GUILayoutOption[] option)
		{
			return EditorGUILayout.BoundsField(content, value, option);
		}
	}
}