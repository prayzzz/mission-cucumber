using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        public bool Toggle(bool value)
        {
            return this.Toggle("", value);
        }
        public bool Toggle(string label, bool value)
        {
            return this.Toggle(label, value, (GUILayoutOption[])null);
        }
        public bool Toggle(string label, bool value, params GUILayoutOption[] option)
        {
            return this.Toggle(label, value, "", option);
        }
        public bool Toggle(string label, bool value, string tooltip)
        {
            return this.Toggle(label, value, tooltip, null);
        }
        public bool Toggle(string label, bool value, string tooltip, params GUILayoutOption[] option)
        {
            return this.Toggle(new GUIContent(label, tooltip), value, option);
        }
        public bool Toggle(GUIContent content, bool value, params GUILayoutOption[] option)
        {
            return this.Toggle(content, value, EditorStyles.toggle, option);
        }
        public bool Toggle(GUIContent content, bool value, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.Toggle(content, value, style, option);
        }
    }
}