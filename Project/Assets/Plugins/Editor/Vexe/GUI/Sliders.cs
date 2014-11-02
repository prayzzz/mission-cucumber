using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public float Slider(float value, float leftValue, float rightValue)
        {
            return this.Slider(value, leftValue, rightValue, null);
        }

        public float Slider(float value, float leftValue, float rightValue, params GUILayoutOption[] option)
        {
            return this.Slider("", value, leftValue, rightValue, option);
        }

        public float Slider(string label, float value, float leftValue, float rightValue)
        {
            return this.Slider(label, value, leftValue, rightValue, null);
        }

        public float Slider(string label, float value, float leftValue, float rightValue, params GUILayoutOption[] option)
        {
            return this.Slider(new GUIContent(label), value, leftValue, rightValue, option);
        }

        public float Slider(GUIContent label, float value, float leftValue, float rightValue, params GUILayoutOption[] option)
        {
            return EditorGUILayout.Slider(label, value, leftValue, rightValue, option);
        }

        public int IntSlider(int value, int leftValue, int rightValue)
        {
            return this.IntSlider(value, leftValue, rightValue, null);
        }

        public int IntSlider(int value, int leftValue, int rightValue, params GUILayoutOption[] option)
        {
            return this.IntSlider("", value, leftValue, rightValue, option);
        }

        public int IntSlider(string label, int value, int leftValue, int rightValue)
        {
            return this.IntSlider(label, value, leftValue, rightValue, null);
        }

        public int IntSlider(string label, int value, int leftValue, int rightValue, params GUILayoutOption[] option)
        {
            return this.IntSlider(new GUIContent(label), value, leftValue, rightValue, option);
        }

        public int IntSlider(GUIContent label, int value, int leftValue, int rightValue, params GUILayoutOption[] option)
        {
            return EditorGUILayout.IntSlider(label, value, leftValue, rightValue, option);
        }
    }
}
