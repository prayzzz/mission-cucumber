using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public int IntField(int value)
        {
            return this.IntField("", value);
        }
        public int IntField(string label, int value)
        {
            return this.IntField(label, value, (GUILayoutOption[])null);
        }
        public int IntField(string label, int value, params GUILayoutOption[] option)
        {
            return this.IntField(label, value, "", option);
        }
        public int IntField(string label, int value, string tooltip)
        {
            return this.IntField(label, value, tooltip, null);
        }
        public int IntField(string label, int value, string tooltip, params GUILayoutOption[] option)
        {
            return this.IntField(GetContent(label, tooltip), value, option);
        }
        public int IntField(GUIContent content, int value, params GUILayoutOption[] option)
        {
            return this.IntField(content, value, EditorStyles.numberField, option);
        }
        public int IntField(GUIContent content, int value, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.IntField(content, value, style, option);
        }
    }
}
