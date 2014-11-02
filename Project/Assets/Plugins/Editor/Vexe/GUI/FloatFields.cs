using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
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
            return this.FloatField("", value);
        }
        public float FloatField(string label, float value)
        {
            return this.FloatField(label, value, (GUILayoutOption[])null);
        }
        public float FloatField(string label, float value, GUILayoutOption[] option)
        {
            return this.FloatField(label, value, "", option);
        }
        public float FloatField(string label, float value, string tooltip)
        {
            return this.FloatField(label, value, tooltip, null);
        }
        public float FloatField(string label, float value, string tooltip, params GUILayoutOption[] option)
        {
            return this.FloatField(GetContent(label, tooltip), value, option);
        }
        public float FloatField(GUIContent content, float value, params GUILayoutOption[] option)
        {
            return this.FloatField(content, value, EditorStyles.numberField, option);
        }
        public float FloatField(GUIContent content, float value, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.FloatField(content, value, option);
        }
    }
}
