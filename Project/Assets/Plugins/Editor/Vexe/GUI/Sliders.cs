using UnityEditor;
using UnityEngine;

namespace Vexe.Editor.Framework.GUIs
{
	public partial class GLWrapper
	{
		public float Slider(float value, float leftValue, float rightValue)
		{
			return Slider(value, leftValue, rightValue, null);
		}

		public float Slider(float value, float leftValue, float rightValue, params GUILayoutOption[] option)
		{
			return Slider("", value, leftValue, rightValue, option);
		}

		public float Slider(string label, float value, float leftValue, float rightValue)
		{
			return Slider(label, value, leftValue, rightValue, null);
		}

		public float Slider(string label, float value, float leftValue, float rightValue, params GUILayoutOption[] option)
		{
			return Slider(new GUIContent(label), value, leftValue, rightValue, option);
		}

		public float Slider(GUIContent label, float value, float leftValue, float rightValue, params GUILayoutOption[] option)
		{
			return EditorGUILayout.Slider(label, value, leftValue, rightValue, option);
		}

		public int IntSlider(int value, int leftValue, int rightValue)
		{
			return IntSlider(value, leftValue, rightValue, null);
		}

		public int IntSlider(int value, int leftValue, int rightValue, params GUILayoutOption[] option)
		{
			return IntSlider("", value, leftValue, rightValue, option);
		}

		public int IntSlider(string label, int value, int leftValue, int rightValue)
		{
			return IntSlider(label, value, leftValue, rightValue, null);
		}

		public int IntSlider(string label, int value, int leftValue, int rightValue, params GUILayoutOption[] option)
		{
			return IntSlider(new GUIContent(label), value, leftValue, rightValue, option);
		}

		public int IntSlider(GUIContent label, int value, int leftValue, int rightValue, params GUILayoutOption[] option)
		{
			return EditorGUILayout.IntSlider(label, value, leftValue, rightValue, option);
		}
	}
}
