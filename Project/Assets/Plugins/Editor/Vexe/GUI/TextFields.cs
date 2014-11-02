using Fasterflect;

using UnityEditor;

using UnityEngine;

namespace Assets.Plugins.Editor.Vexe.GUI
{
    public partial class GLWrapper
    {
        private MethodInvoker toolbarSearchField;

        public GLWrapper()
        {
            this.toolbarSearchField = typeof(EditorGUILayout).DelegateForCallMethod("ToolbarSearchField", Flags.StaticPrivate, typeof(string), typeof(GUILayoutOption[]));
        }

        public string TextField(string value)
        {
            return this.TextField(string.Empty, value);
        }
        public string TextField(string label, string value)
        {
            return this.TextField(label, value, (GUILayoutOption[])null);
        }
        public string TextField(string label, string value, params GUILayoutOption[] option)
        {
            return this.TextField(label, value, string.Empty, option);
        }
        public string TextField(string label, string value, string tooltip)
        {
            return this.TextField(label, value, tooltip, null);
        }
        public string TextField(string label, string value, string tooltip, params GUILayoutOption[] option)
        {
            return this.TextField(GetContent(label, tooltip), value, option);
        }
        public string TextField(GUIContent content, string value, params GUILayoutOption[] option)
        {
            return this.TextField(content, value, EditorStyles.numberField, option);
        }
        public string TextField(GUIContent content, string value, GUIStyle style, params GUILayoutOption[] option)
        {
            return EditorGUILayout.TextField(content, value, style, option);
        }

        public string ToolbarSearchField(string current)
        {
            return this.ToolbarSearchField(current, null);
        }
        public string ToolbarSearchField(string current, params GUILayoutOption[] options)
        {
            return this.toolbarSearchField(null, current, options) as string;
        }
    }
}
