using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public Vector3 Vector3Field(Vector3 value)
		{
			return Vector3Field("", value);
		}
		public Vector3 Vector3Field(string label, Vector3 value)
		{
			return Vector3Field(label, value, null);
		}
		public Vector3 Vector3Field(string label, string tooltip, Vector3 value)
		{
			return Vector3Field(label, tooltip, value, null);
		}
		public Vector3 Vector3Field(string label, Vector3 value, params GUILayoutOption[] option)
		{
			return Vector3Field(label, "", value, option);
		}
		public Vector3 Vector3Field(string label, string tooltip, Vector3 value, params GUILayoutOption[] option)
		{
			return Vector3Field(GetContent(label, tooltip), value, option);
		}
		public Vector3 Vector3Field(GUIContent content, Vector3 value, params GUILayoutOption[] option)
		{
			return EditorGUILayout.Vector3Field(content, value, option);
		}

		public Vector2 Vector2Field(Vector2 value)
		{
			return Vector2Field("", value);
		}
		public Vector2 Vector2Field(string label, Vector2 value)
		{
			return Vector2Field(label, value, null);
		}
		public Vector2 Vector2Field(string label, string tooltip, Vector2 value)
		{
			return Vector2Field(label, tooltip, value, null);
		}
		public Vector2 Vector2Field(string label, Vector2 value, params GUILayoutOption[] option)
		{
			return Vector2Field(label, "", value, option);
		}
		public Vector2 Vector2Field(string label, string tooltip, Vector2 value, params GUILayoutOption[] option)
		{
			return Vector2Field(GetContent(label, tooltip), value, option);
		}
		public Vector2 Vector2Field(GUIContent content, Vector2 value, params GUILayoutOption[] option)
		{
			return EditorGUILayout.Vector2Field(content, value, option);
		}
	}
}
